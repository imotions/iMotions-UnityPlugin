using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Vive
{
    [System.Serializable]
    public class EyeTrackingData
    {
        public float timeStamp;
        public float gazeLeftX;
        public float gazeLeftY;
        public float pupilDiaLeft;
        public float gazeRightX;
        public float gazeRightY;
        public float pupilDiaRight;
    }
}