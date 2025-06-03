using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Coflow.iMotionsPlugin.Meta
{

    public class EyeGazeConverter_Meta : EyeGazeConverter
    {

        protected override void Update()
        {
            if (Physics.Raycast(this.transform.position, this.transform.forward, out raycastHit, 10f))
            {
                hitPoint = raycastHit.point;
            }
            else
            {
                hitPoint = this.transform.position + this.transform.forward * 10f;
            }

            pupilDiam = 0; //no pupil data on OVREyeGaze

            if (visualDebugReference != null)
                visualDebugReference.transform.position = hitPoint;

            currentEyePointInUnityCoords = mainCam.WorldToViewportPoint(hitPoint, Camera.MonoOrStereoscopicEye.Mono);

            currentEyePointIniMotionsCoords.x = currentEyePointInUnityCoords.x * maxX;
            currentEyePointIniMotionsCoords.y = (1 - currentEyePointInUnityCoords.y) * maxY;
        }
    }
}