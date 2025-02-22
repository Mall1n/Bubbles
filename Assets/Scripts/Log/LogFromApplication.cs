using UnityEngine;


namespace Game
{
    public class LogFromApplication : MonoBehaviour
    {
        [SerializeField] private bool _logStackTrace = false;
        
        private void Awake()
        {
            if (!Debug.isDebugBuild)
            {
                Log.SetFileDateTime();
                Log.CheckFolderPath();
                Log.WriteDirectlyInFile("Application Start");
                Log.LogStackTrace = _logStackTrace;
            }
        }

        private void OnEnable()
        {
            if (!Debug.isDebugBuild)
                Application.logMessageReceived += Log.LogCallbackAsync;
        }

        private void OnDisable()
        {
            if (!Debug.isDebugBuild)
                Application.logMessageReceived -= Log.LogCallbackAsync;
        }
    }
}

