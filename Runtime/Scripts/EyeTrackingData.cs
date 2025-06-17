using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Meta
{

    [System.Serializable]
    public class EyeTrackingData
    {
        public float timeStamp;
        //Left lookat
        public float gazeLeftX;
        public float gazeLeftY;
        //Left pupil
        public float pupilDiaLeft;
        //Right lookat
        public float gazeRightX;
        public float gazeRightY;
        //Right pupil
        public float pupilDiaRight;

        //Left Blinking
        public float leftEyeBlinking;
        //Right Blinking
        public float rightEyeBlinking;
    }
}