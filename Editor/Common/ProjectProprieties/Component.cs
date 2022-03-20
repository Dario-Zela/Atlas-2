using System.Diagnostics;
using System.Runtime.Serialization;

namespace Editor
{
    public interface IMSComponent { }

    [DataContract]
    public abstract class Component : ViewModelBase
    {
        [DataMember]
        public GameEntity Owner { get; private set; }

        public Component(GameEntity entity)
        {
            Debug.Assert(entity != null);
            Owner = entity;
        }
    }

    abstract class MSComponent<T> : ViewModelBase, IMSComponent where T : Component
    {

    }
}
