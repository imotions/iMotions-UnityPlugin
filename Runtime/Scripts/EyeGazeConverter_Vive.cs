using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
#if VIVE_OPENXR
using VIVE.OpenXR;
using VIVE.OpenXR.EyeTracker;
using VIVE.OpenXR.FacialTracking;
#endif
namespace Coflow.iMotionsPlugin.Vive
{

    public class EyeGazeConverter_Vive : EyeGazeConverter
    {
        protected float pupilPositionX = 0f;
        protected float pupilPositionY = 0f;
        protected float eyeOpenness = 0f;
        protected float eyeSqueeze = 0f;
        protected float eyeWide = 0f;
        protected float blinking = 0f;

        public float GetBlinking()
        {
            return blinking;
        }
        public float GetPupilPositionX()
        {
            return pupilPositionX;
        }
        public float GetPupilPositionY()
        {
            return pupilPositionY;
        }
        public float GetEyeOpenness()
        {
            return eyeOpenness;
        }

        public float GetEyeSqueeze()
        {
            return eyeSqueeze;
        }

        public float GetEyeWide()
        {
            return eyeWide;
        }

#if VIVE_OPENXR

        protected float[] eyeExps = new float[(int)XrEyeExpressionHTC.XR_EYE_EXPRESSION_MAX_ENUM_HTC];
        protected ViveFacialTracking viveFacialTracking;

        protected void Start()
        {
            viveFacialTracking = OpenXRSettings.Instance.GetFeature<ViveFacialTracking>();
        }

        protected override void Update()
        {
            XR_HTC_eye_tracker.Interop.GetEyeGazeData(out XrSingleEyeGazeDataHTC[] out_gazes);
            XR_HTC_eye_tracker.Interop.GetEyePupilData(out XrSingleEyePupilDataHTC[] out_pupils);
            XR_HTC_eye_tracker.Interop.GetEyeGeometricData(out XrSingleEyeGeometricDataHTC[] out_geometric);

            //XR_HTC_facial_tracking.Interop.xrGetFacialExpressionsHTC()

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
                if(leftPupil.isPositionValid)
                {
                    pupilPositionX = leftPupil.pupilPosition.x;
                    pupilPositionY = leftPupil.pupilPosition.y;
                }
                else
                {
                    pupilPositionX = 0f;
                    pupilPositionY = 0f;
                }
                XrSingleEyeGeometricDataHTC leftGeometric = out_geometric[(int)XrEyePositionHTC.XR_EYE_POSITION_LEFT_HTC];
                if (leftGeometric.isValid)
                {
                    eyeOpenness = leftGeometric.eyeOpenness;
                    eyeSqueeze = leftGeometric.eyeSqueeze;
                    eyeWide = leftGeometric.eyeWide;
                }
                else
                {
                    eyeOpenness = 0f;
                    eyeSqueeze = 0f;
                    eyeWide = 0f;
                }

                if (viveFacialTracking != null)
                {
                    if (viveFacialTracking.GetFacialExpressions(XrFacialTrackingTypeHTC.XR_FACIAL_TRACKING_TYPE_EYE_DEFAULT_HTC, out float[] exps))
                    {
                        eyeExps = exps;

                        blinking = eyeExps[(int)XrEyeExpressionHTC.XR_EYE_EXPRESSION_LEFT_BLINK_HTC];
                    }
                    else
                    {
                        blinking = 0;
                    }
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
                if (rightPupil.isPositionValid)
                {
                    pupilPositionX = rightPupil.pupilPosition.x;
                    pupilPositionY = rightPupil.pupilPosition.y;
                }
                else
                {
                    pupilPositionX = 0f;
                    pupilPositionY = 0f;
                }

                XrSingleEyeGeometricDataHTC rightGeometric = out_geometric[(int)XrEyePositionHTC.XR_EYE_POSITION_RIGHT_HTC];
                if (rightGeometric.isValid)
                {
                    eyeOpenness = rightGeometric.eyeOpenness;
                    eyeSqueeze = rightGeometric.eyeSqueeze;
                    eyeWide = rightGeometric.eyeWide;
                }
                else
                {
                    eyeOpenness = 0f;
                    eyeSqueeze = 0f;
                    eyeWide = 0f;
                }

                if (viveFacialTracking != null)
                {
                    if (viveFacialTracking.GetFacialExpressions(XrFacialTrackingTypeHTC.XR_FACIAL_TRACKING_TYPE_EYE_DEFAULT_HTC, out float[] exps))
                    {
                        eyeExps = exps;

                        blinking = eyeExps[(int)XrEyeExpressionHTC.XR_EYE_EXPRESSION_RIGHT_BLINK_HTC];
                    }
                    else
                    {
                        blinking = 0;
                    }
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