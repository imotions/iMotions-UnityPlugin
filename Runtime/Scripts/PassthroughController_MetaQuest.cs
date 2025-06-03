using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Meta
{

    [RequireComponent(typeof(OVRPassthroughLayer))]
    public class PassthroughController_MetaQuest : PassthroughController
    {
        OVRPassthroughLayer passthroughLayer;

        private void Awake()
        {
            passthroughLayer = GetComponent<OVRPassthroughLayer>();
        }

        void Start()
        {
            passthroughLayer.textureOpacity = 0f;
        }

        public override void EnablePassthrough()
        {
            passthroughLayer.textureOpacity = 1f;
        }

        public override void DisablePassthrough()
        {
            passthroughLayer.textureOpacity = 0f;
        }
    }
}