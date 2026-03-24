using TMPro;
using UnityEngine;

namespace Bubbles
{
    public class TouchesInfo : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private LidarTouchesHandler _lidarTouchesHandler;

        [Header("Output")]
        [SerializeField] private TextMeshProUGUI[] _touchesTMP;
        [SerializeField] private TextMeshProUGUI _touchesCount;

        [Header("Settings")]
        [SerializeField] private bool _enableOutput = true;

        private float _timeLastUpdatedMark = -1f;
        private bool _textsIsEmpty = true;

        private void OnEnable()
        {
            _lidarTouchesHandler.dataTouchesUpdated += OnTouchesUpdated;
        }

        private void OnDisable()
        {
            _lidarTouchesHandler.dataTouchesUpdated -= OnTouchesUpdated;
        }

        private void Update()
        {
            if (!_textsIsEmpty && Time.time - _timeLastUpdatedMark > 1f)
            {
                ClearAllTexts();
            }
        }

        private void OnTouchesUpdated(TouchData[] touches)
        {
            if (!_enableOutput)
                return;
            if (touches == null || touches.Length == 0)
            {
                ClearAllTexts();
                return;
            }

            _timeLastUpdatedMark = Time.time;
            _textsIsEmpty = false;

            _touchesCount.text = touches.Length.ToString();

            int tochesCountLog = touches.Length > _touchesTMP.Length ? _touchesTMP.Length : touches.Length;

            for (int i = 0; i < tochesCountLog; i++)
            {
                _touchesTMP[i].text = touches[i].ToString();
            }

            for (int i = touches.Length; i < _touchesTMP.Length; i++)
            {
                _touchesTMP[i].text = "";
            }
        }

        private void ClearAllTexts()
        {
            _touchesCount.text = "0";

            for (int i = 0; i < _touchesTMP.Length; i++)
            {
                _touchesTMP[i].text = "";
                _textsIsEmpty = true;
            }
        }
    }
}