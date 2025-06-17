using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Vive
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

        //Left Pupil Pos
        public float pupilPositionLeftX = 0f;
        public float pupilPositionLeftY = 0f;
        //Right pupil pos
        public float pupilPositionRightX = 0f;
        public float pupilPositionRightY = 0f;
        //Eye openness left/right
        public float eyeOpennessLeft = 0f;
        public float eyeOpennessRight = 0f;
        //Eye squeeze left/right
        public float eyeSqueezeLeft = 0f;
        public float eyeSqueezeRight = 0f;
        //Eye wide left/right
        public float eyeWideLeft = 0f;
        public float eyeWideRight = 0f;
    }
}