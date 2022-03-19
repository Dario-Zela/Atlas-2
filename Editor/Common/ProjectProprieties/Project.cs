using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;

namespace Editor
{
    [DataContract]
    public class Project : ViewModelBase
    {
        public static string Extention => ".at";
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

        [DataMember]
        public string Path { get; private set; }
        public string FullPath => $"{Path}{Name}{Extention}";

        [DataMember(Name = "ActiveScene")]
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

        [DataMember(Name = "Scenes")]
        private readonly ObservableCollection<Scene> _scenes = new();
        public ReadOnlyObservableCollection<Scene>? Scenes { get; private set; }

        public static Project? Current => Application.Current.MainWindow.DataContext as Project;

        public static Project? Load(string filepath)
        {
            Debug.Assert(File.Exists(filepath));
            return Serialiser.FromFile<Project>(filepath);
        }
        public void Unload()
        {
            Application.Current.MainWindow.DataContext = null;
        }

        public static void Save(Project? project)
        {
            Debug.Assert(project != null);
            Serialiser.ToFile(project, project.FullPath);
        }
        public static void Save()
        {
            Save(Current);
        }

        public void ChangePath(string filepath)
        {
            Path = filepath;
        }
        private void InternalAddScene(string sceneName, int pos = -1)
        {
            Debug.Assert(!string.IsNullOrEmpty(sceneName.Trim()));
            _scenes.Insert(pos < 0 ? Scenes!.Count : pos, new Scene(this, sceneName));
        }
        private void InternalRemoveScene(Scene scene)
        {
            Debug.Assert(_scenes.Contains(scene));
            _scenes.Remove(scene);
        }

        public void AddScene(string sceneName)
        {
            InternalAddScene(sceneName);
            UndoRedoManager.Add(new UndoRedoAction(
                "New Scene",
                () => { InternalRemoveScene(Scenes!.Last()); },
                () => { InternalAddScene(sceneName, Scenes!.Count - 1); }
                ));
        }
        public void RemoveScene(Scene scene)
        {
            int pos = Scenes!.IndexOf(scene);
            InternalRemoveScene(scene);
            UndoRedoManager.Add(new UndoRedoAction(
                "New Scene",
                () => { InternalAddScene(scene.Name, pos); },
                () => { InternalRemoveScene(scene); }
                ));
        }

        public ICommand? AddSceneCommand { get; private set; }
        public ICommand? RemoveSceneCommand { get; private set; }

        [OnDeserialized]
        public void OnDeserialised(StreamingContext context)
        {
            if (_scenes != null)
            {
                Scenes = new(_scenes);
                OnProprietyChanged(nameof(Scenes));
            }

            AddSceneCommand = new RelayCommand<string>(sceneName => { AddScene(sceneName); }, null);
            RemoveSceneCommand = new RelayCommand<Scene>(scene => { RemoveScene(scene); }, null);
        }

        public Project(string name, string path)
        {
            _name = name;
            Path = path;

            OnDeserialised(new StreamingContext());
        }
    }
}
