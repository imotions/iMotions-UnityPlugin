using TMPro;
using UnityEditor;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Meta
{
    /// <summary>
    /// The sample's eye tracking labels use TextMeshPro, which needs the TMP Essential Resources in the project.
    /// Without them every label throws a NullReferenceException and stays blank, so import them when they're missing.
    /// </summary>
    [InitializeOnLoad]
    public static class TMPEssentialsImporter
    {
        const string SessionKey = "iMotionsPlugin.TMPEssentialsChecked";

        static TMPEssentialsImporter()
        {
            if (SessionState.GetBool(SessionKey, false))
                return;
            EditorApplication.delayCall += ImportIfMissing;
        }

        public static void ImportIfMissing()
        {
            SessionState.SetBool(SessionKey, true);
            if (AssetDatabase.FindAssets("t:TMP_Settings").Length > 0)
                return;

            Debug.Log("iMotions plugin: importing TMP Essential Resources, which the sample's eye tracking labels need.");
            TMP_PackageResourceImporter.ImportResources(true, false, false);
        }
    }
}
