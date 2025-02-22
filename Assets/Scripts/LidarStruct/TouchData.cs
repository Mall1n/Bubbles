using System;
using System.Collections.Generic;
using System.Text;


namespace Game
{
    [Serializable]
    public struct TouchData
    {
        public List<byte> lidarID;
        public byte touchID;
        public CustomPoint normalizedTouchPoint;
        public float touchSize;
        public string command;

        public override string ToString()
        {
            return $"lidarID: [{GetLidarsID()}] | TouchID: {touchID} | normalizedTouchPoint: {normalizedTouchPoint.ToString(5)} | touchSize: {touchSize.ToString().Truncate(5)} | command: {command}";
        }

        private string GetLidarsID()
        {
            if (lidarID == null || lidarID.Count == 0)
                return "[]";

            StringBuilder stringBuilder = new($"[{lidarID[0]}]");

            for (int i = 1; i < lidarID.Count; i++)
            {
                stringBuilder.Insert(1, $", {lidarID[i]}");
            }

            return stringBuilder.ToString();
        }
    }
}