using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Vive
{
    public class EyetrackingDataRetriever : MonoBehaviour
    {
        [Header("Events:")]
        [SerializeField] private GameEvent_EyeTrackingData onEyetrackingDataSend;

        [Header("Eye Gaze Converters:")]
        [SerializeField] private EyeGazeConverter leftGazeConverter;
        [SerializeField] private EyeGazeConverter rightGazeConverter;

        [Header("(Optional) Visualizing Values Texts:")]
        [SerializeField] private TMP_Text leftEyeDirectionNormalizedText;
        [SerializeField] private TMP_Text leftEyePupilDiameterText;
        [SerializeField] private TMP_Text rightEyeDirectionNormalizedText;
        [SerializeField] private TMP_Text rightEyePupilDiameterText;


        Vector3 leftEyeDirectionNormalized = new Vector3();
        Vector3 rightEyeDirectionNormalized = new Vector3();

        float leftEyePupilDiameter;
        float rightEyePupilDiameter;

        private bool recording;
        private int startingTime;

        public void SetupResolution(int width, int height)
        {
            leftGazeConverter.SetupResolution(width, height);
            rightGazeConverter.SetupResolution(width, height);
        }

        private void Update()
        {
            leftEyeDirectionNormalized.x = leftGazeConverter.GetEyeX();
            leftEyeDirectionNormalized.y = leftGazeConverter.GetEyeY();

            rightEyeDirectionNormalized.x = rightGazeConverter.GetEyeX();
            rightEyeDirectionNormalized.y = rightGazeConverter.GetEyeY();

            if (leftEyeDirectionNormalizedText != null)
                leftEyeDirectionNormalizedText.text = leftEyeDirectionNormalized.ToString("F2");

            if (rightEyeDirectionNormalizedText != null)
                rightEyeDirectionNormalizedText.text = rightEyeDirectionNormalized.ToString("F2");
        }

        void FixedUpdate()
        {
            if (recording)
            {
                EyeTrackingData dataSample = new EyeTrackingData();

                dataSample.timeStamp = Mathf.RoundToInt(startingTime);
                startingTime += Mathf.RoundToInt(Time.deltaTime * 1000f);

                leftEyeDirectionNormalized.x = leftGazeConverter.GetEyeX();
                leftEyeDirectionNormalized.y = leftGazeConverter.GetEyeY();

                rightEyeDirectionNormalized.x = rightGazeConverter.GetEyeX();
                rightEyeDirectionNormalized.y = rightGazeConverter.GetEyeY();

                leftEyePupilDiameter = leftGazeConverter.GetPupilDiam();
                rightEyePupilDiameter = rightGazeConverter.GetPupilDiam();

                dataSample.gazeLeftX = leftGazeConverter.GetEyeX();
                dataSample.gazeLeftY = leftGazeConverter.GetEyeY();
                dataSample.pupilDiaLeft = leftEyePupilDiameter;

                dataSample.gazeRightX = rightGazeConverter.GetEyeX();
                dataSample.gazeRightY = rightGazeConverter.GetEyeY();
                dataSample.pupilDiaRight = rightEyePupilDiameter;

                if (leftEyeDirectionNormalizedText != null)
                    leftEyeDirectionNormalizedText.text = leftEyeDirectionNormalized.ToString("F2");

                if (rightEyeDirectionNormalizedText != null)
                    rightEyeDirectionNormalizedText.text = rightEyeDirectionNormalized.ToString("F2");

                if (leftEyePupilDiameterText != null)
                    leftEyePupilDiameterText.text = leftEyePupilDiameter.ToString("F2");

                if (rightEyePupilDiameterText != null)
                    rightEyePupilDiameterText.text = rightEyePupilDiameter.ToString("F2");

                onEyetrackingDataSend.Raise(dataSample);
            }

        }

        public void StartRecording()
        {
            recording = true;
            startingTime = 0;
        }

        public void StopRecording()
        {
            recording = false;
        }
    }
}