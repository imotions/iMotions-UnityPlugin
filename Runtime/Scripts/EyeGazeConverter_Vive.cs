using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
#if VIVE_OPENXR
using VIVE.OpenXR;
using VIVE.OpenXR.EyeTracker;
#endif
namespace Coflow.iMotionsPlugin.Vive
{
    public class EyeGazeConverter_Vive : EyeGazeConverter
    {
#if VIVE_OPENXR
        protected override void Update()
        {
            XR_HTC_eye_tracker.Interop.GetEyeGazeData(out XrSingleEyeGazeDataHTC[] out_gazes);
            XR_HTC_eye_tracker.Interop.GetEyePupilData(out XrSingleEyePupilDataHTC[] out_pupils);

            if (out_gazes == null)
            {
                Debug.LogError("Vive Eyetracking missing or not initialized properly");
                return;
            }

            if (eye == Camera.MonoOrStereoscopicEye.Left)
            {
                XrSingleEyeGazeDataHTC leftGaze = out_gazes[(int)XrEyePositionHTC.XR_EYE_POSITION_LEFT_HTC];
                if (leftGaze.isValid)
                {
                    transform.position = leftGaze.gazePose.position.ToUnityVector();
                    transform.rotation = leftGaze.gazePose.orientation.ToUnityQuaternion();
                }

                XrSingleEyePupilDataHTC leftPupil = out_pupils[(int)XrEyePositionHTC.XR_EYE_POSITION_LEFT_HTC];
                if (leftPupil.isDiameterValid)
                {
                    pupilDiam = leftPupil.pupilDiameter;
                }
                else
                {
                    pupilDiam = 0f;
                }
            }
            else
            {
                XrSingleEyeGazeDataHTC rightGaze = out_gazes[(int)XrEyePositionHTC.XR_EYE_POSITION_RIGHT_HTC];
                if (rightGaze.isValid)
                {
                    transform.position = rightGaze.gazePose.position.ToUnityVector();
                    transform.rotation = rightGaze.gazePose.orientation.ToUnityQuaternion();
                }

                XrSingleEyePupilDataHTC rightPupil = out_pupils[(int)XrEyePositionHTC.XR_EYE_POSITION_RIGHT_HTC];
                if (rightPupil.isDiameterValid)
                {
                    pupilDiam = rightPupil.pupilDiameter;
                }
                else
                {
                    pupilDiam = 0f;
                }
            }


            if (Physics.Raycast(mainCam.transform.position, this.transform.forward, out raycastHit, 10f))
            {
                hitPoint = raycastHit.point;
            }
            else
            {
                hitPoint = mainCam.transform.position + this.transform.forward * 10f;
            }

            if (visualDebugReference != null)
                visualDebugReference.transform.position = hitPoint;

            currentEyePointInUnityCoords = mainCam.WorldToViewportPoint(hitPoint, Camera.MonoOrStereoscopicEye.Mono);

            currentEyePointIniMotionsCoords.x = currentEyePointInUnityCoords.x * maxX;
            currentEyePointIniMotionsCoords.y = (1 - currentEyePointInUnityCoords.y) * maxY;


        }
#endif
    }
}