using System.Linq;
using UnityEditor;
using UnityEditor.Build;
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
                // NamedBuildTarget replaces the BuildTargetGroup overloads, which are obsolete in Unity 6.
                var target = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                string symbols = PlayerSettings.GetScriptingDefineSymbols(target);
                if (!symbols.Contains("VIVE_OPENXR"))
                {
                    symbols += ";VIVE_OPENXR";
                    PlayerSettings.SetScriptingDefineSymbols(target, symbols);
                }
            }
        }
    }
}