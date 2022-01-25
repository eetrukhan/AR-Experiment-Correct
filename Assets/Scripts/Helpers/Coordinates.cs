using UnityEngine;

namespace Logic
{
    public class Coordinates
    {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;

        public Coordinates(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public Vector3 Position { get { return position; } }
        public Quaternion Rotation { get { return rotation; } }
        public Vector3 Scale { get { return scale; } }

        public override string ToString()
        {
            return "Position: " + Position.ToString() + " Rotation: " + Rotation.ToString() + " Scale: " + Scale.ToString();
        }
    }
}