using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProMovieCapture;

namespace Coflow.iMotionsPlugin.Vive
{
    public class CaptureController : MonoBehaviour
    {
        CaptureFromCamera captureComp;

        private void Awake()
        {
            captureComp = GetComponent<CaptureFromCamera>();

            captureComp.SetCamera(Camera.main, false);
            captureComp.UseContributingCameras = false;
            captureComp.CameraRenderResolution = CaptureBase.Resolution.Custom;
        }

        public void SetupResolution(int width, int height)
        {
            captureComp.UseContributingCameras = false;
            captureComp.CameraRenderResolution = CaptureBase.Resolution.Custom;
            captureComp.CameraRenderCustomResolution = new Vector2(width, height);
        }

        public void StartRecording()
        {
            Debug.Log("#video start capture");
            captureComp.StartCapture();
        }


        public void EndRecording()
        {
            Debug.Log("#video stop capture");
            captureComp.StopCapture();
        }

        public void SetOutputPath(string _subFolderPath)
        {
            captureComp.OutputFolderPath = _subFolderPath;
        }
    }
}