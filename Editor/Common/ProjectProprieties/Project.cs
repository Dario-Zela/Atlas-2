using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using Editor.Browser;

namespace Editor
{
    //Implements the view model base for a project
    [DataContract]
    public class Project : ViewModelBase
    {
        //The extention for the project file
        public static string Extention => ".at";

        //The field and property for the project's name
        private string _name;
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnProprietyChanged(nameof(Name));
                }
            }
        }

        //The field and property for the project's path
        [DataMember]
        public string Path { get; private set; }
        public string FullPath => $"{Path}{Name}{Extention}";

        //The currently active scene, saved as it's location in the array
        [DataMember(Name = nameof(ActiveScene))]
        public int _activeScenePos;
        public Scene ActiveScene
        {
            get => _scenes[_activeScenePos];
            set
            {
                int pos = _scenes.IndexOf(value);

                if (pos != _activeScenePos && pos != -1)
                {
                    _activeScenePos = pos;
                    OnProprietyChanged(nameof(ActiveScene));
                }
            }
        }

        //The container of scenes in the project
        [DataMember(Name = nameof(Scenes))]
        private ObservableCollection<Scene> _scenes = new();
        public ReadOnlyObservableCollection<Scene>? Scenes { get; private set; }

        //The currently active project
        public static Project? Current => Application.Current.MainWindow.DataContext as Project;

        //Loads the project from the path
        public static Project? Load(string filepath)
        {
            Logger.Log(MessageType.Trace, $"Loading project from path {filepath}");

            //Makes sure the file exits
            Debug.Assert(File.Exists(filepath));
            //And deserialises the project from the file
            return Serialiser.FromFile<Project>(filepath);
        }
        //Unloads the current project
        public static void Unload()
        {
            Logger.Log(MessageType.Trace, "Unloading project");

            //Removes the current application's data context
            Application.Current.MainWindow.DataContext = null;

            //Resets the Undo Redo manager
            UndoRedoManager.Reset();
        }

        //Saves the project
        public static void Save(Project? project)
        {
            Logger.Log(MessageType.Trace, $"Saving project {project?.Name}");

            //Makes sure the project isn't null
            Debug.Assert(project != null);

            //And saves the current file data and file to disk
            ExistingProjects.Save(project);
            Serialiser.ToFile(project, project.FullPath);
        }
        //Saves the current project
        public static void Save()
        {
            Save(Current);
        }

        //Chenges the path of the project
        public void ChangePath(string filepath)
        {
            Logger.Log(MessageType.Trace, $"Changing path of project {Name} from {Path} to {filepath}");

            Path = filepath;
        }

        //Internal addition of a new scene
        private Scene InternalAddNewScene(string sceneName)
        {
            Logger.Log(MessageType.Trace, $"Adding scene {sceneName} to project {Name}");

            //Make sure the name is valid
            Debug.Assert(!string.IsNullOrEmpty(sceneName.Trim()));

            //Create the new scene and add it to the container
            var scene = new Scene(this, sceneName);
            _scenes.Add(scene);

            //Return it
            return scene;
        }
        //Internal insertion of a scene at a location
        private void InternalInsertScene(Scene scene, uint pos)
        {
            Logger.Log(MessageType.Trace, $"Adding scene {scene.Name} to project {Name} in position {pos}");

            //Making sure scene can be placed in that position
            Debug.Assert(_scenes!.Count >= pos);

            //Insert the scene in that position
            _scenes.Insert((int)pos, scene);
        }
        //Internal removal of a scene 
        private void InternalRemoveScene(Scene scene)
        {
            Logger.Log(MessageType.Trace, $"Removing scene {scene.Name} from project {Name}");

            //Make sure the scene is in _scenes
            Debug.Assert(_scenes.Contains(scene));

            //Remove the scene
            _scenes.Remove(scene);
        }

        //Wrap the internal scene to allow for undoing and redoing
        //The action
        public void AddScene(string sceneName)
        {
            //Add the new scene and save it
            var scene = InternalAddNewScene(sceneName);
            uint scenePosition = (uint)Scenes!.IndexOf(scene);

            //On undo remove the scene and on redo
            //Insert the scene back to it's original location
            UndoRedoManager.Add(new UndoRedoAction(
                $"Add Scene {sceneName} to Project {Name}",
                () => { InternalRemoveScene(scene); },
                () => { InternalInsertScene(scene, scenePosition); }
                ));
        }
        public void RemoveScene(Scene scene)
        {
            //Get the position of the scene and remove it
            uint pos = (uint)Scenes!.IndexOf(scene);
            InternalRemoveScene(scene);

            //On redo insert the scene back to it's original location and on undo
            //Remove the scene
            UndoRedoManager.Add(new UndoRedoAction(
                $"Remove Scene {scene.Name} from Project {Name}",
                () => { InternalInsertScene(scene, pos); },
                () => { InternalRemoveScene(scene); }
                ));
        }

        //Commands to add and remove scenes from a project
        public ICommand? AddSceneCommand { get; private set; }
        public ICommand? RemoveSceneCommand { get; private set; }

        //The method run post deserialisation
        [OnDeserialized]
        public void OnDeserialised(StreamingContext context)
        {
            Logger.Log(MessageType.Trace, $"Deserialising the Project {Name}");

            //Make sure scenes isn't null
            if (_scenes == null)
            {
                _scenes = new();
            }

            //Generate the property and invoke the property changed event
            Scenes = new(_scenes);
            OnProprietyChanged(nameof(Scenes));

            //Generate the commands
            AddSceneCommand = new RelayCommand<string>(sceneName => { AddScene(sceneName); }, null);
            RemoveSceneCommand = new RelayCommand<Scene>(scene => { RemoveScene(scene); }, null);
        }

        public Project(string name, string path)
        {
            //Set the name and path
            _name = name;
            Path = path;

            //Run the OnDeserialised method
            OnDeserialised(new StreamingContext());
        }
    }
}
