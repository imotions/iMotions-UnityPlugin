using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Meta
{
    /// <summary>
    /// AVPro Movie Capture from the Asset Store gives Unity 6 warnings in a new project:
    /// - its files are saved by an old Unity version, so Unity warns "contains a ... object at version N,
    ///   below the supported minimum" every time it imports them;
    /// - its scripts call FindFirstObjectByType and FindObjectsByType(FindObjectsSortMode), which Unity 6.6
    ///   marks as obsolete (CS0618), and a demo script has an unused field (CS0414).
    /// Fix both once, in the AVPro folder of the project.
    /// </summary>
    [InitializeOnLoad]
    public static class AVProFormatUpgrader
    {
        // CaptureFromCamera.cs in the paid AVPro Movie Capture (Desktop and Mobile editions).
        const string CaptureFromCameraGuid = "f41e58c74463edf43a9624fcdf483d19";
        const string SessionKey = "iMotionsPlugin.AVProFormatChecked";

        static readonly string[] ScriptsWithObsoleteCalls =
        {
            "Runtime/Scripts/Internal/CaptureBase.cs",
            "Editor/Scripts/CaptureEditorWindow.cs",
        };
        const string DemoScriptWithUnusedField = "Demos/Scripts/ScriptCaptureDemo.cs";
        const string UnusedFieldPragma = "#pragma warning disable 0414 // _drawGUI is assigned but never used (added by the iMotions plugin)";

        static readonly Regex SortModeCall = new Regex(
            @"^([ \t]*)([^\r\n]*?= )FindObjectsByType<(\w+)>\(FindObjectsSortMode\.None\);", RegexOptions.Multiline);

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

            if (PatchScripts(root))
                AssetDatabase.Refresh();

            ResaveOldFiles(root);
        }

        static bool PatchScripts(string root)
        {
            bool changed = false;
            foreach (string rel in ScriptsWithObsoleteCalls)
            {
                changed |= PatchFile(Path.Combine(root, rel), text =>
                {
                    string newline = text.Contains("\r\n") ? "\r\n" : "\n";
                    // FindAnyObjectByType exists in every Unity version that has FindFirstObjectByType.
                    string patched = text.Replace("FindFirstObjectByType<", "FindAnyObjectByType<");
                    // The overload without a sort mode only exists from Unity 6.6, so keep the old call for older versions.
                    if (!patched.Contains("#if UNITY_6000_6_OR_NEWER"))
                    {
                        patched = SortModeCall.Replace(patched, m =>
                            "#if UNITY_6000_6_OR_NEWER" + newline +
                            m.Groups[1].Value + m.Groups[2].Value + "FindObjectsByType<" + m.Groups[3].Value + ">();" + newline +
                            "#else" + newline +
                            m.Value + newline +
                            "#endif");
                    }
                    return patched;
                });
            }

            changed |= PatchFile(Path.Combine(root, DemoScriptWithUnusedField), text =>
            {
                if (!text.Contains("_drawGUI") || text.Contains(UnusedFieldPragma))
                    return text;
                string newline = text.Contains("\r\n") ? "\r\n" : "\n";
                return UnusedFieldPragma + newline + text;
            });

            if (changed)
                Debug.Log("iMotions plugin: updated AVPro Movie Capture's scripts to the current Unity API, so the obsolete API warnings stop.");
            return changed;
        }

        // Rewrites the file when the patch changes it, keeping its byte order mark.
        static bool PatchFile(string path, System.Func<string, string> patch)
        {
            if (!File.Exists(path))
                return false;
            byte[] bytes = File.ReadAllBytes(path);
            bool bom = bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;
            string text = new UTF8Encoding(false).GetString(bytes, bom ? 3 : 0, bytes.Length - (bom ? 3 : 0));
            string patched = patch(text);
            if (patched == text)
                return false;
            File.WriteAllText(path, patched, new UTF8Encoding(bom));
            return true;
        }

        static void ResaveOldFiles(string root)
        {
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
