using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Meta
{
    public class EyeBlinkRetriever : MonoBehaviour
    {
        protected float leftEyeBlinking;
        protected float rightEyeBlinking;

        public float GetLeftEyeBlinking()
        {
            return leftEyeBlinking;
        }
        public float GetRightEyeBlinking()
        {
            return rightEyeBlinking;
        }

        protected virtual void Awake()
        {

        }

        protected virtual void Update()
        {

        }
    }
}
