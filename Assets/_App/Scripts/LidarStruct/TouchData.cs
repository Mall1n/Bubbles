using System;

namespace Bubbles
{
    [Serializable]
    public struct TouchData
    {
        public TouchPoint posNormalized;
        public float touchSize;
        public State state;
        public string id;

        public enum State
        {
            New = 0,
            Active = 1,
            Remove = 3
        }

        public override string ToString()
        {
            return $"normalizedTouchPoint: {posNormalized,-16} | touchSize: {touchSize,-5:F1} | state: {state,-7} | id: {id}";
        }
    }
}