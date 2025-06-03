using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if VIVE_OPENXR
using VIVE.OpenXR.Passthrough;
#endif
namespace Coflow.iMotionsPlugin.Vive
{
    public class PassthroughController_Vive : PassthroughController
    {
#if VIVE_OPENXR
        [Header("Recommended for iMotions usage is Overlay\nIf you want to use underlay,\nmake sure your project properly supports Mixed Reality with VIVE OpenXR SDK")]
        [SerializeField] private VIVE.OpenXR.CompositionLayer.LayerType layerType;

        XrPassthroughHTC xrPassthrough;
#endif
        void Start()
        {
#if VIVE_OPENXR
            PassthroughAPI.CreatePlanarPassthrough(out xrPassthrough, layerType);
            PassthroughAPI.SetPassthroughAlpha(xrPassthrough, 0);
#endif
        }

        public override void EnablePassthrough()
        {
#if VIVE_OPENXR
            PassthroughAPI.SetPassthroughAlpha(xrPassthrough, 1);
#endif
        }

        public override void DisablePassthrough()
        {
#if VIVE_OPENXR
            PassthroughAPI.SetPassthroughAlpha(xrPassthrough, 0);
#endif
        }

        private void OnDestroy()
        {
#if VIVE_OPENXR
            PassthroughAPI.DestroyPassthrough(xrPassthrough);
#endif
        }
    }
}