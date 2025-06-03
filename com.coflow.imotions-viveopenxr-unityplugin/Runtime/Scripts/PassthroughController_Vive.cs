using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIVE.OpenXR.Passthrough;

namespace Coflow.iMotionsPlugin.Vive
{
    public class PassthroughController_Vive : PassthroughController
    {
        [Header("Recommended for iMotions usage is Overlay\nIf you want to use underlay,\nmake sure your project properly supports Mixed Reality with VIVE OpenXR SDK")]
        [SerializeField] private VIVE.OpenXR.CompositionLayer.LayerType layerType;

        XrPassthroughHTC xrPassthrough;

        void Start()
        {
            PassthroughAPI.CreatePlanarPassthrough(out xrPassthrough, layerType);
            PassthroughAPI.SetPassthroughAlpha(xrPassthrough, 0);
        }

        public override void EnablePassthrough()
        {
            PassthroughAPI.SetPassthroughAlpha(xrPassthrough, 1);
        }

        public override void DisablePassthrough()
        {
            PassthroughAPI.SetPassthroughAlpha(xrPassthrough, 0);
        }

        private void OnDestroy()
        {
            PassthroughAPI.DestroyPassthrough(xrPassthrough);
        }
    }
}