using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Vive
{
    /// <summary>
    /// The sample's eye tracking labels use TextMeshPro, which needs the TMP Essential Resources in the project.
    /// Without them every label throws a NullReferenceException and stays blank, so import them when they're missing.
    /// TMP's font ships with import settings from an old Unity version, which Unity 6 warns about
    /// ("below the supported minimum"), so re-save them in the current format after the import.
    /// </summary>
    [InitializeOnLoad]
    public static class TMPEssentialsImporter
    {
        const string SessionKey = "iMotionsPlugin.TMPEssentialsChecked";

        static readonly Regex OldFontImporter = new Regex(
            @"TrueTypeFontImporter:[ \t]*\n(?:[ \t]+\S.*\n){0,3}?[ \t]+serializedVersion: [0-3][ \t]*\n");

        static TMPEssentialsImporter()
        {
            AssetDatabase.importPackageCompleted -= OnPackageImported;
            AssetDatabase.importPackageCompleted += OnPackageImported;

            if (SessionState.GetBool(SessionKey, false))
                return;
            EditorApplication.delayCall += ImportIfMissing;
        }

        public static void ImportIfMissing()
        {
            SessionState.SetBool(SessionKey, true);
            if (AssetDatabase.FindAssets("t:TMP_Settings").Length > 0)
            {
                ResaveOldFonts();
                return;
            }

            Debug.Log("iMotions plugin: importing TMP Essential Resources, which the sample's eye tracking labels need.");
            TMP_PackageResourceImporter.ImportResources(true, false, false);
        }

        static void OnPackageImported(string packageName)
        {
            if (packageName.Contains("TMP Essential Resources"))
                ResaveOldFonts();
        }

        public static void ResaveOldFonts()
        {
            string[] oldFonts = AssetDatabase.FindAssets("t:Font", new[] { "Assets" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Distinct()
                .Where(p => p.Contains("/TextMesh Pro/") && File.Exists(p + ".meta")
                    && OldFontImporter.IsMatch(File.ReadAllText(p + ".meta").Replace("\r\n", "\n")))
                .ToArray();
            if (oldFonts.Length == 0)
                return;

            AssetDatabase.ForceReserializeAssets(oldFonts, ForceReserializeAssetsOptions.ReserializeMetadata);
            Debug.Log($"iMotions plugin: re-saved {oldFonts.Length} TextMesh Pro font(s) in the current Unity format, so the 'below the supported minimum' warning stops.");
        }
    }
}
