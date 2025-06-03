using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Vive
{
    public class EyeGazeConverter : MonoBehaviour
    {
        protected const float minX = 0;
        protected const float minY = 0;
        protected float maxX;
        protected float maxY;

        [Header("Left or Right:")]
        [SerializeField] protected Camera.MonoOrStereoscopicEye eye;

        [Header("(Optional) Visual Reference For Debugging:")]
        [SerializeField] protected Transform visualDebugReference;

        protected Vector3 currentEyePointInUnityCoords = new Vector3();
        protected Vector3 currentEyePointIniMotionsCoords = new Vector3();
        protected float pupilDiam;

        protected Vector3 hitPoint = new Vector3();
        protected RaycastHit raycastHit;

        protected Camera mainCam;

        protected void Awake()
        {
            mainCam = Camera.main;
        }

        public void SetupResolution(int x, int y)
        {
            maxX = x;
            maxY = y;
        }


        protected virtual void Update()
        {

        }

        // Remap function to map a value from one range to another range.
        protected float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        public float GetEyeX()
        {
            return Mathf.Clamp(currentEyePointIniMotionsCoords.x, minX, maxX);
        }
        public float GetEyeY()
        {
            return Mathf.Clamp(currentEyePointIniMotionsCoords.y, minY, maxY);
        }
        public float GetPupilDiam()
        {
            return pupilDiam;
        }
    }
}