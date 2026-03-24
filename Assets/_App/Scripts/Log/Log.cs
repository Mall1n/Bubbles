using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;


namespace Bubbles
{
    public static class Log
    {
        private static string _logDirectoryPath = $"{Application.dataPath}/Logs";
        private static string _dateTimeLog;


        private static string _logFilePath => $"{_logDirectoryPath}/{(_dateTimeLog == null ? "" : $"{_dateTimeLog} ")}log.txt";
        public static bool LogStackTrace = false;


        public static void CheckFolderPath()
        {
            if (!Directory.Exists(_logDirectoryPath))
                Directory.CreateDirectory(_logDirectoryPath);
        }

        public static void SetFileDateTime()
        {
            _dateTimeLog = DateTime.Now.ToString("G").Replace(':', '-');
        }

        public static void Write(string text)
        {
            if (Debug.isDebugBuild)
                Debug.Log(text);
            else
                Task.Run(() => LogInFile(FormatText(text, LogType.Log)));
        }

        public static void Write(string text, LogType logType)
        {
            if (Debug.isDebugBuild)
                DebugLog(text, logType);
            else
                Task.Run(() => LogInFile(FormatText(text, logType)));
        }

        public static void WriteDirectlyInFile(string text)
        {
            if (!Debug.isDebugBuild)
                Task.Run(() => LogInFile(FormatText(text, LogType.Log)));
        }

        public static void WriteDirectlyInFile(string text, LogType logType)
        {
            if (!Debug.isDebugBuild)
                Task.Run(() => LogInFile(FormatText(text, logType)));
        }

        private static void DebugLog(string text, LogType logType)
        {
            switch (logType)
            {
                case LogType.Warning:
                    Debug.LogWarning(text);
                    break;
                case LogType.Error:
                    Debug.LogError(text);
                    break;
                case LogType.Exception:
                    Debug.LogException(new Exception(text));
                    break;
                default:
                    Debug.Log(text);
                    break;
            }
        }

        private static string FormatText(string info, LogType logType)
        {
            if (logType == LogType.Log)
                return $"[{System.DateTime.Now}]: {info}";
            else
                return $"[{System.DateTime.Now}] [{logType}]: {info}";
        }

        public static void LogCallbackAsync(string condition, string stackTrace, LogType type) => Task.Run(() => LogCallbackAsyncTaskRun(condition, stackTrace, type));

        private static async Task LogCallbackAsyncTaskRun(string condition, string stackTrace, LogType type)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    await writer.WriteLineAsync(FormatText(condition, type));

                    if (LogStackTrace && (type == LogType.Error || type == LogType.Exception))
                    {
                        await writer.WriteLineAsync(stackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to write log.\n{ex}");
            }
        }

        private static async Task LogInFile(string text)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    await writer.WriteLineAsync(text);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to write log.\n{ex}");
            }
        }
    }
}