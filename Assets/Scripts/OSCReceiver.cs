using System;
using UnityEngine;


namespace Game
{
    public class OSCReceiver : MonoBehaviour
    {
        [SerializeField] private extOSC.OSCReceiver _receiver;
        [SerializeField] private string _address = "";
        [SerializeField] private bool _logReceivedMessage = true;


        public event Action<extOSC.OSCMessage> OnReceivedMessage;


        private void Awake()
        {
            if (_receiver == null)
            {
                Log.Write("OSCReceiver is null", LogType.Warning);
                this.enabled = false;
                return;
            }

            _receiver.Bind(_address, MessageReceived);
        }

        private void MessageReceived(extOSC.OSCMessage message)
        {
            // if (message == null)
            //     return;

            if (_logReceivedMessage)
                Log.Write(message.ToString());

            OnReceivedMessage?.Invoke(message);
        }
    }
}
