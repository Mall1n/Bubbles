using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Game
{
    public class TouchesInfo : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private LidarTouchesHandler _referenceLidarsData;

        [Header("Output")]
        [SerializeField] private TextMeshProUGUI[] _touches;
        [SerializeField] private TextMeshProUGUI _touchesCount;

        [Header("Settings")]
        [SerializeField] private bool _enableOutput = true;


        private void Awake()
        {
            if (_referenceLidarsData == null)
                Log.Write($"{nameof(_referenceLidarsData)} is null", LogType.Warning);

            if (_touchesCount == null)
                Log.Write($"{nameof(_touchesCount)} is null", LogType.Warning);

            if (_touches == null)
                Log.Write($"{nameof(_touches)} is null", LogType.Warning);
        }

        void Update()
        {
            if (_enableOutput)
                OutputTouches();
        }

        private void OutputTouches()
        {

            if (_referenceLidarsData.Touches == null)
            {
                _touchesCount.text = "null";
                return;
            }

            _touchesCount.text = _referenceLidarsData.Touches.Count.ToString();

            int tochesCountLog = _referenceLidarsData.Touches.Count > 10 ? 10 : _referenceLidarsData.Touches.Count;

            for (int i = 0; i < tochesCountLog; i++)
            {
                if (_touches[i] != null)
                    _touches[i].text = _referenceLidarsData.Touches[i].ToString();
            }

            for (int i = _referenceLidarsData.Touches.Count; i < _touches.Length; i++)
            {
                if (_touches[i] != null)
                    _touches[i].text = "";
            }

        }
    }
}