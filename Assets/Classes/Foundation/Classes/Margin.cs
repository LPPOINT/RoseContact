using System;

namespace Assets.Classes.Foundation.Classes
{
    [Serializable]
    public class Margin
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public Margin()
        {
            
        }

        public Margin(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}
