namespace KISS_GAME
{
    public struct Vector
    {
        private float[] vec;

        public float this[int i]
        {
            get => vec[i];
            set => vec[i] = value;
        }

        public Vector()
        { 
            vec = new float[4];
        }

        public Vector(float x, float y, float z)
        { 
            vec = new float[] { x, y, z };
        }
    }
}
