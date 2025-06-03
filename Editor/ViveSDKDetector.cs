using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;

namespace Coflow.iMotionsPlugin.Vive
{
    [InitializeOnLoad]
    public static class ViveSDKDetector
    {
        static ViveSDKDetector()
        {
            var assemblies = CompilationPipeline.GetAssemblies();
            bool found = assemblies.Any(a => a.name.Contains("VIVE.OpenXR")); // Or a known class name
            if (found)
            {
                string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                if (!symbols.Contains("VIVE_OPENXR"))
                {
                    symbols += ";VIVE_OPENXR";
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
                }
            }
        }
    }
}