using System;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct CustomPoint
    {
        public double x;
        public double y;


        public override string ToString()
        {
            return $"x: {x}, y: {y}";
        }

        public string ToString(int truncateValueString)
        {
            return $"x: {x.ToString().Truncate(truncateValueString)}, y: {y.ToString().Truncate(truncateValueString)}";
        }

        public static explicit operator Vector3(CustomPoint v)
        {
            return new Vector3((float)v.x, (float)v.y, 0);
        }

        public static CustomPoint operator *(CustomPoint c, Vector2 v)
        {
            return new CustomPoint() { x = c.x * v.x, y = c.y * v.y };
        }
    }
}