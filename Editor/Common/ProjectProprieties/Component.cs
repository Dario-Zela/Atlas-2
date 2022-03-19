using System.Diagnostics;
using System.Runtime.Serialization;

namespace Editor
{
    [DataContract]
    public class Component : ViewModelBase
    {
        [DataMember]
        public GameEntity Owner { get; private set; }

        public Component(GameEntity entity)
        {
            Debug.Assert(entity != null);
            Owner = entity;
        }
    }
}
