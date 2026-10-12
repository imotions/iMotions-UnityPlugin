using System;
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
    /// <summary>
    /// Converts HTC gaze poses to Unity world space relative to the tracked head pose.
    /// This preserves the head-relative gaze direction when the XR Origin is moved or rotated.
    /// Assumes HTC gaze poses and Unity's XRNode.Head pose share the same tracking space.
    /// </summary>
    public class EyeGazeConverter_Vive : EyeGazeConverter
    {
        protected float pupilPositionX = 0f;
        protected float pupilPositionY = 0f;
        protected float eyeOpenness = 0f;
        protected float eyeSqueeze = 0f;
        protected float eyeWide = 0f;
        protected float blinking = 0f;

        public float GetBlinking() { return blinking; }
        public float GetPupilPositionX() { return pupilPositionX; }
        public float GetPupilPositionY() { return pupilPositionY; }
        public float GetEyeOpenness() { return eyeOpenness; }
        public float GetEyeSqueeze() { return eyeSqueeze; }
        public float GetEyeWide() { return eyeWide; }

#if VIVE_OPENXR
        protected float[] eyeExps =
            new float[(int)XrEyeExpressionHTC.XR_EYE_EXPRESSION_MAX_ENUM_HTC];
        protected ViveFacialTracking viveFacialTracking;

        private InputDevice headDevice;
        private bool warnedAboutHeadTracking;

        protected void Start()
        {
            viveFacialTracking = OpenXRSettings.Instance.GetFeature<ViveFacialTracking>();
            headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        }

        protected override void Update()
        {
            if (mainCam == null)
                return;

            XR_HTC_eye_tracker.Interop.GetEyeGazeData(out XrSingleEyeGazeDataHTC[] out_gazes);
            XR_HTC_eye_tracker.Interop.GetEyePupilData(out XrSingleEyePupilDataHTC[] out_pupils);
            XR_HTC_eye_tracker.Interop.GetEyeGeometricData(out XrSingleEyeGeometricDataHTC[] out_geometric);

            if (out_gazes == null || out_gazes.Length < 2)
                return;

            if (!headDevice.isValid)
                headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

            Vector3 trackedHeadPosition;
            Quaternion trackedHeadRotation;
            bool hasPosition = headDevice.TryGetFeatureValue(
                CommonUsages.devicePosition, out trackedHeadPosition);
            bool hasRotation = headDevice.TryGetFeatureValue(
                CommonUsages.deviceRotation, out trackedHeadRotation);

            if (!hasPosition || !hasRotation)
            {
                if (!warnedAboutHeadTracking)
                {
                    Debug.LogWarning("EyeGazeConverter_Vive: Head tracking pose is unavailable.");
                    warnedAboutHeadTracking = true;
                }
                return;
            }
            warnedAboutHeadTracking = false;

            bool isLeftEye = eye == Camera.MonoOrStereoscopicEye.Left;
            int eyeIndex = isLeftEye
                ? (int)XrEyePositionHTC.XR_EYE_POSITION_LEFT_HTC
                : (int)XrEyePositionHTC.XR_EYE_POSITION_RIGHT_HTC;

            if (eyeIndex < 0 || eyeIndex >= out_gazes.Length)
                return;

            XrSingleEyeGazeDataHTC gaze = out_gazes[eyeIndex];
            if (!gaze.isValid)
                return;

            Vector3 rawEyePosition = gaze.gazePose.position.ToUnityVector();
            Quaternion rawEyeRotation = gaze.gazePose.orientation.ToUnityQuaternion();

            // Remove the tracked head pose so that the gaze is head-relative.
            Quaternion inverseHeadRotation = Quaternion.Inverse(trackedHeadRotation);
            Vector3 headRelativeEyePosition = inverseHeadRotation *
                (rawEyePosition - trackedHeadPosition);
            Quaternion headRelativeEyeRotation = inverseHeadRotation * rawEyeRotation;

            // Reapply that relative pose in the actual rendered XR camera's world space.
            Vector3 worldEyePosition = mainCam.transform.TransformPoint(headRelativeEyePosition);
            Quaternion worldEyeRotation =
                mainCam.transform.rotation * headRelativeEyeRotation;
            transform.SetPositionAndRotation(worldEyePosition, worldEyeRotation);

            if (out_pupils != null && out_pupils.Length > eyeIndex)
            {
                XrSingleEyePupilDataHTC pupil = out_pupils[eyeIndex];
                pupilDiam = pupil.isDiameterValid ? pupil.pupilDiameter : 0f;
                if (pupil.isPositionValid)
                {
                    pupilPositionX = pupil.pupilPosition.x;
                    pupilPositionY = pupil.pupilPosition.y;
                }
                else
                {
                    pupilPositionX = 0f;
                    pupilPositionY = 0f;
                }
            }

            if (out_geometric != null && out_geometric.Length > eyeIndex)
            {
                XrSingleEyeGeometricDataHTC geometry = out_geometric[eyeIndex];
                if (geometry.isValid)
                {
                    eyeOpenness = geometry.eyeOpenness;
                    eyeSqueeze = geometry.eyeSqueeze;
                    eyeWide = geometry.eyeWide;
                }
                else
                {
                    eyeOpenness = 0f;
                    eyeSqueeze = 0f;
                    eyeWide = 0f;
                }
            }

            if (viveFacialTracking != null &&
                viveFacialTracking.GetFacialExpressions(
                    XrFacialTrackingTypeHTC.XR_FACIAL_TRACKING_TYPE_EYE_DEFAULT_HTC,
                    out float[] exps))
            {
                eyeExps = exps;
                int blinkIndex = isLeftEye
                    ? (int)XrEyeExpressionHTC.XR_EYE_EXPRESSION_LEFT_BLINK_HTC
                    : (int)XrEyeExpressionHTC.XR_EYE_EXPRESSION_RIGHT_BLINK_HTC;
                blinking = eyeExps[blinkIndex];
            }
            else
            {
                blinking = 0f;
            }

            Vector3 gazeOrigin = worldEyePosition;
            Vector3 gazeDirection = (worldEyeRotation * Vector3.forward).normalized;

            if (Physics.Raycast(gazeOrigin, gazeDirection, out raycastHit, 10f))
                hitPoint = raycastHit.point;
            else
                hitPoint = gazeOrigin + gazeDirection * 10f;

            if (visualDebugReference != null)
                visualDebugReference.transform.position = hitPoint;

            currentEyePointInUnityCoords =
                mainCam.WorldToViewportPoint(hitPoint, Camera.MonoOrStereoscopicEye.Mono);
            currentEyePointIniMotionsCoords.x = currentEyePointInUnityCoords.x * maxX;
            currentEyePointIniMotionsCoords.y = (1f - currentEyePointInUnityCoords.y) * maxY;
        }
#endif
    }
}
