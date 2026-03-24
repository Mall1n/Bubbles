using System;
using UnityEngine;

namespace Bubbles
{
    [Serializable]
    public struct TouchPoint
    {
        public float x;
        public float y;

        public TouchPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"x: {x:F2}, y: {y:F2}";
        }

        // public string ToString(int truncateValueString)
        // {
        //     return $"x: {x.ToString().Truncate(truncateValueString)}, y: {y.ToString().Truncate(truncateValueString)}";
        // }

        public static explicit operator Vector2(TouchPoint v)
        {
            return new Vector2((float)v.x, (float)v.y);
        }

        public static explicit operator Vector3(TouchPoint v)
        {
            return new Vector3((float)v.x, (float)v.y, 0);
        }

        public static TouchPoint operator *(TouchPoint c, Vector2 v)
        {
            return new TouchPoint() { x = c.x * v.x, y = c.y * v.y };
        }
    }
}