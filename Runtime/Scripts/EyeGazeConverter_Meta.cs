using UnityEngine;
using UnityEngine.XR;

namespace Coflow.iMotionsPlugin.Meta
{
    /// <summary>
    /// Converts Meta gaze poses to Unity world space relative to the tracked head pose.
    /// This preserves the head-relative gaze direction when the camera rig (OVRCameraRig or XR Origin)
    /// is moved or rotated, and no longer depends on where this object or its parent sits in the scene.
    /// Assumes Meta gaze poses and Unity's XRNode.Head pose share the same tracking space.
    /// OVREyeGaze on this object still starts eye tracking and sets the confidence threshold,
    /// but it no longer moves this transform; this component sets it from the converted gaze.
    /// </summary>
    public class EyeGazeConverter_Meta : EyeGazeConverter
    {
        private const float DefaultConfidenceThreshold = 0.5f;

        private OVREyeGaze ovrEyeGaze;
        private OVRPlugin.EyeGazesState eyeGazesState;
        private InputDevice headDevice;
        private bool warnedAboutHeadTracking;

        protected void Start()
        {
            ovrEyeGaze = GetComponent<OVREyeGaze>();
            if (ovrEyeGaze != null)
            {
                // OVREyeGaze adds the start rotation of this object and of its parent to the gaze,
                // which offsets it. It keeps eye tracking running, but the pose is set here instead.
                ovrEyeGaze.ApplyPosition = false;
                ovrEyeGaze.ApplyRotation = false;
            }

            headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        }

        protected override void Update()
        {
            if (mainCam == null)
                return;

            if (!OVRPlugin.GetEyeGazesState(OVRPlugin.Step.Render, -1, ref eyeGazesState))
                return;

            if (eyeGazesState.EyeGazes == null || eyeGazesState.EyeGazes.Length < 2)
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
                    Debug.LogWarning("EyeGazeConverter_Meta: Head tracking pose is unavailable.");
                    warnedAboutHeadTracking = true;
                }
                return;
            }
            warnedAboutHeadTracking = false;

            bool isLeftEye = eye == Camera.MonoOrStereoscopicEye.Left;
            int eyeIndex = isLeftEye ? (int)OVRPlugin.Eye.Left : (int)OVRPlugin.Eye.Right;

            if (eyeIndex < 0 || eyeIndex >= eyeGazesState.EyeGazes.Length)
                return;

            OVRPlugin.EyeGazeState gaze = eyeGazesState.EyeGazes[eyeIndex];
            float confidenceThreshold = ovrEyeGaze != null
                ? ovrEyeGaze.ConfidenceThreshold
                : DefaultConfidenceThreshold;
            if (!gaze.IsValid || gaze.Confidence < confidenceThreshold)
                return;

            OVRPose rawEyePose = gaze.Pose.ToOVRPose();
            Vector3 rawEyePosition = rawEyePose.position;
            Quaternion rawEyeRotation = rawEyePose.orientation;

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

            pupilDiam = 0f; // Meta eye tracking provides no pupil data.

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
    }
}
