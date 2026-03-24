using System;
using UnityEngine;

namespace Bubbles
{
    public class LidarTouchesHandler : MonoBehaviour
    {
        [SerializeField] private OSCReceiver _oSCReceiver;

        public event Action<TouchData[]> dataTouchesUpdated;

        private void OnEnable()
        {
            _oSCReceiver.dataReceived += OSCReceiver_OnDataReceived;
        }

        private void OSCReceiver_OnDataReceived(TouchData[] touchesData)
        {
            dataTouchesUpdated?.Invoke(touchesData);
        }

        private void OnDisable()
        {
            _oSCReceiver.dataReceived += OSCReceiver_OnDataReceived;
        }
    }
}
