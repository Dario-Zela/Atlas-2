using System.Runtime.Serialization;

namespace Editor
{
    //Generates the Multi Scelection componet interface
    public interface IMSComponent { }

    //Implements the view model base for a Component
    [DataContract]
    public abstract class Component : ViewModelBase
    {
        //The entity that owns this component
        [DataMember]
        public GameEntity Owner { get; private set; }

        public Component(GameEntity entity)
        {
            //Set the owner to the entity
            Owner = entity;
        }
    }

    abstract class MSComponent<T> : ViewModelBase, IMSComponent where T : Component { }
}
