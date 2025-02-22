using System;
using System.Collections.Generic;
using UnityEngine;

using OSCMessage = extOSC.OSCMessage;


namespace Game
{
    public static class OSCMessageCommand
    {
        public static readonly string Create = "Create";
        public static readonly string Update = "Update";
        public static readonly string Delete = "Delete";
    }

    public class LidarTouchesHandler : MonoBehaviour
    {
        [SerializeField] private OSCReceiver _oSCReceiver;


        private List<TouchData> _touches = new();
        public IReadOnlyList<TouchData> Touches => _touches;

        private const string OSCValueString = "(OSCValue(String) : \"";


        private void OnEnable()
        {
            if (_oSCReceiver != null)
                _oSCReceiver.OnReceivedMessage += GetOSCMessage;
        }

        void OnDisable()
        {
            if (_oSCReceiver != null)
                _oSCReceiver.OnReceivedMessage -= GetOSCMessage;
        }

        private void CheckForNonUpdatedTouches()
        {
            // 
        }

        private void GetOSCMessage(OSCMessage message)
        {
            if (message == null)
                return;

            string jsonString = "";

            try // add code exception
            {
                int indexStart = message.ToString().IndexOf(OSCValueString);
                indexStart += OSCValueString.Length;

                if (indexStart != -1)
                {
                    jsonString = message.ToString().Substring(indexStart, message.ToString().Length - indexStart - 2);

                    // Log.Write($"jsonString = {jsonString}");
                }
                else
                {
                    Log.Write($"index is out of string range in jsonString");
                    return;
                }
            }
            catch (System.Exception e)
            {
                Log.Write(e.Message);
            }

            try
            {
                LidarData lidarsData = JsonUtility.FromJson<LidarData>(jsonString);

                if (lidarsData.touches != null)
                    HandleTouchData(lidarsData);
            }
            catch (ArgumentNullException e)
            {
                Log.Write(e.Message);
            }
        }

        private void HandleTouchData(LidarData lidarsData)
        {
            for (int i = 0; i < lidarsData.touches.Count; i++)
            {
                TouchData touchDataNew = lidarsData.touches[i];

                if (touchDataNew.command == OSCMessageCommand.Create)
                {
                    _touches.Add(touchDataNew);
                }
                else if (touchDataNew.command == OSCMessageCommand.Update)
                {
                    int indexTouchUpdate = _touches.FindIndex(_ => _.touchID == touchDataNew.touchID);
                    if (indexTouchUpdate == -1)
                        _touches.Add(touchDataNew);
                    else
                        _touches[indexTouchUpdate] = touchDataNew;
                }
                else if (touchDataNew.command == OSCMessageCommand.Delete)
                {
                    int indexTouchDelete = _touches.FindIndex(_ => _.touchID == touchDataNew.touchID);
                    if (indexTouchDelete != -1)
                        _touches.RemoveAt(indexTouchDelete);
                }
            }
        }
    }
}
