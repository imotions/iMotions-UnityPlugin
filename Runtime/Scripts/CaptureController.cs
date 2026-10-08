using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProMovieCapture;

namespace Coflow.iMotionsPlugin.Vive
{
    public class CaptureController : MonoBehaviour
    {
        CaptureFromCamera captureComp;

        private const string fileName = "EyeTrackingDataFile";

        string timestamp;

        private void Awake()
        {
            captureComp = GetComponent<CaptureFromCamera>();

            captureComp.SetCamera(Camera.main, false);
            InitCapture();
        }

        public void SetupResolution(int width, int height)
        {
            InitCapture();
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

        public void SetOutputPath(string _subFolderPath, string timestamp)
        {
            captureComp.OutputFolderPath = _subFolderPath;
            this.timestamp = timestamp;
            captureComp.FilenamePrefix = fileName + "-" + timestamp;
        }
        private void InitCapture()
        {
            captureComp.UseContributingCameras = false;
            captureComp.CameraRenderResolution = CaptureBase.Resolution.Custom;

            captureComp.AppendFilenameTimestamp = false;
            captureComp.AllowManualFileExtension = false;
            captureComp.FilenamePrefix = fileName + "-" + timestamp;
        }
    }
}