using UnityEngine;

namespace Reclaimers.DeveloperConsole
{
    public static class LogSystem
    {
        public static void Log(object msg)
        {
            // TODO - print to console



            Debug.Log(msg);
        }

        public static void LogError(object msg)
        {
            // TODO - print to console


            Debug.LogError(msg);
        }
    }
}

