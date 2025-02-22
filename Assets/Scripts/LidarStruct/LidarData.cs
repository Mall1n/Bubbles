using System;
using System.Collections.Generic;


namespace Game
{
    [Serializable]
    public struct LidarData
    {
        public int uid;
        public List<TouchData> touches;

        public override string ToString()
        {
            return base.ToString();
        }
    }
}