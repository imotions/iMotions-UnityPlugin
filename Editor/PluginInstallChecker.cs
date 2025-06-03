using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Meta
{
    [InitializeOnLoad]
    public static class PluginInstallChecker
    {
        static PluginInstallChecker()
        {
            // This runs immediately after domain reload
            CheckAVProPathExists();
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            // This runs only once per script reload
            CheckAVProPathExists();
        }

        private static void CheckAVProPathExists()
        {
            string path = "Assets/Plugins/RenderHeads/AVProMovieCapture/Editor/";

            if (!Directory.Exists(path))
            {
                Debug.LogWarning($"[PluginSetup] Path missing: {path}");
                // You can also create it if needed:
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
                Debug.Log($"[PluginSetup] Created missing directory: {path}");
            }
            else
            {
                Debug.Log($"[PluginSetup] Verified: {path} exists");
            }
        }
    }
}