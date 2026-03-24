using Tracking.Protos;
using UnityEngine;
using Google.Protobuf.Collections;
using System;

namespace Bubbles
{
    public class OSCReceiver : MonoBehaviour
    {
        [SerializeField] private UDPProtobufReceiver uDPProtobufReceiver;

        public event Action<TouchData[]> dataReceived;

        private void OnEnable()
        {
            uDPProtobufReceiver.OnDataReceived -= OSCReceiver_OnDataReceived;
            uDPProtobufReceiver.OnDataReceived += OSCReceiver_OnDataReceived;
        }

        private void OnDisable()
        {
            uDPProtobufReceiver.OnDataReceived -= OSCReceiver_OnDataReceived;
        }

        private void OSCReceiver_OnDataReceived(TrackedObjectsBatchProto protoBatch)
        {
            RepeatedField<TrackedObjectProto> fields = protoBatch.Objects;

            int count = protoBatch.Objects.Count;
            TouchData[] touches = new TouchData[count];

            for (int i = 0; i < count; i++)
            {
                TrackedObjectProto proto = fields[i];
                TouchData touchData = new TouchData()
                {
                    posNormalized = new TouchPoint(proto.X, proto.Y),
                    state = (TouchData.State)(int)proto.State,
                    touchSize = proto.CurrentCluster.Area,
                    id = proto.ObjectId
                };

                touches[i] = touchData;
            }

            dataReceived?.Invoke(touches);
        }
    }
}
