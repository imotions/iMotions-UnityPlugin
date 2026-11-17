using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Vive
{
    /// <summary>
    /// AVPro Movie Capture from the Asset Store ships files saved by an old Unity version, so Unity 6 warns
    /// "contains a ... object at version N, below the supported minimum" every time it imports them.
    /// Re-save the AVPro folder once in the current format so the warnings stop.
    /// </summary>
    [InitializeOnLoad]
    public static class AVProFormatUpgrader
    {
        // CaptureFromCamera.cs in the paid AVPro Movie Capture (Desktop and Mobile editions).
        const string CaptureFromCameraGuid = "f41e58c74463edf43a9624fcdf483d19";
        const string SessionKey = "iMotionsPlugin.AVProFormatChecked";

        static AVProFormatUpgrader()
        {
            if (SessionState.GetBool(SessionKey, false))
                return;
            EditorApplication.delayCall += UpgradeIfNeeded;
        }

        public static void UpgradeIfNeeded()
        {
            SessionState.SetBool(SessionKey, true);

            string scriptPath = AssetDatabase.GUIDToAssetPath(CaptureFromCameraGuid);
            int index = scriptPath.IndexOf("/AVProMovieCapture/");
            if (index < 0 || !scriptPath.StartsWith("Assets/"))
                return;
            string root = scriptPath.Substring(0, index + "/AVProMovieCapture".Length);

            // The Asset Store package ships its native plugin import settings at PluginImporter version 1,
            // which Unity 6 warns about. Once re-saved they're at a newer version, so this runs only once.
            var oldPluginImporter = new Regex(@"PluginImporter:[ \t]*\n(?:[ \t]+\S.*\n){0,3}?[ \t]+serializedVersion: 1[ \t]*\n");
            bool hasOldFiles = Directory.EnumerateFiles(root, "*.meta", SearchOption.AllDirectories)
                .Any(f => oldPluginImporter.IsMatch(File.ReadAllText(f).Replace("\r\n", "\n")));
            if (!hasOldFiles)
                return;

            string[] paths = AssetDatabase.FindAssets("", new[] { root })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Distinct()
                .ToArray();
            AssetDatabase.ForceReserializeAssets(paths, ForceReserializeAssetsOptions.ReserializeAssetsAndMetadata);
            Debug.Log($"iMotions plugin: re-saved {paths.Length} AVPro Movie Capture files in the current Unity format, so the 'below the supported minimum' warnings stop.");
        }
    }
}
