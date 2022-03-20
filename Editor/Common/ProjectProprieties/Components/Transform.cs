using System.Numerics;
using System.Runtime.Serialization;

namespace Editor
{
    //The implementation of a transform component
    [DataContract]
    public class Transform : Component
    {
        //Field and property for the position of the owner
        private Vector3 _position;
        [DataMember]
        public Vector3 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnProprietyChanged(nameof(Position));
                }
            }
        }

        //Field and property for the rotation of the owner
        private Vector3 _rotation;
        [DataMember]
        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    OnProprietyChanged(nameof(Rotation));
                }
            }
        }

        //Field and property for the scale of the owner
        private Vector3 _scale;
        [DataMember]
        public Vector3 Scale
        {
            get => _scale;
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    OnProprietyChanged(nameof(Scale));
                }
            }
        }

        public Transform(GameEntity entity) : base(entity)
        {
        }
    }
}
