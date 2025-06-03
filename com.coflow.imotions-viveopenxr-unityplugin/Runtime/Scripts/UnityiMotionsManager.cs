using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Coflow.iMotionsPlugin.Vive
{
    public class UnityiMotionsManager : MonoBehaviour
    {
        [Header("Video Capture Resolution Settings:")]
        [SerializeField] private int width;
        [SerializeField] private int height;

        [Header("In case you want to use UI buttons instead of controller input:\n\nCall StartRecording()/StopRecording()\nand EnablePassthrough()/DisablePassthrough()\nthrough an onclick UI event.")]
        [Header("Input references for Toggling Video capture and Passthrough:")]
        [SerializeField] private InputActionReference optionalToggleRecordingInputKey;
        [SerializeField] private InputActionReference optionalTogglePassthroughInputKey;
        [Header("Optional:")]
        [SerializeField] private GameObject isRecordingDebugVisualCue;

        private CaptureController captureController;
        private EyeTrackingFileManager eyetrackingManager;
        private EyetrackingDataRetriever eyetrackingDataRetriever;
        private PassthroughController passthroughController;

        private bool isRecording = false;
        private bool passthroughEnabled = false;

        private void Awake()
        {
            captureController = FindObjectOfType<CaptureController>();
            eyetrackingManager = FindObjectOfType<EyeTrackingFileManager>();
            eyetrackingDataRetriever = FindObjectOfType<EyetrackingDataRetriever>();
            passthroughController = FindObjectOfType<PassthroughController>();
        }

        private void Start()
        {
            captureController.SetupResolution(width, height);
            eyetrackingDataRetriever.SetupResolution(width, height);
        }

        private void OnEnable()
        {
            if (optionalToggleRecordingInputKey != null && optionalToggleRecordingInputKey.action != null)
            {
                optionalToggleRecordingInputKey.action.performed += OnToggleRecording;
                optionalToggleRecordingInputKey.action.Enable();
            }

            if (optionalTogglePassthroughInputKey != null && optionalTogglePassthroughInputKey.action != null)
            {
                optionalTogglePassthroughInputKey.action.performed += OnTogglePassthrough;
                optionalTogglePassthroughInputKey.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (optionalToggleRecordingInputKey != null && optionalToggleRecordingInputKey.action != null)
            {
                optionalToggleRecordingInputKey.action.performed -= OnToggleRecording;
                optionalToggleRecordingInputKey.action.Disable();
            }

            if (optionalTogglePassthroughInputKey != null && optionalTogglePassthroughInputKey.action != null)
            {
                optionalTogglePassthroughInputKey.action.performed += OnTogglePassthrough;
                optionalTogglePassthroughInputKey.action.Enable();
            }
        }

        private void OnToggleRecording(InputAction.CallbackContext context)
        {
            if (isRecording)
            {
                StopRecording();
            }
            else
            {
                StartRecording();
            }
            isRecording = !isRecording;
            if (isRecordingDebugVisualCue != null)
                isRecordingDebugVisualCue.SetActive(isRecording);
        }

        private void OnTogglePassthrough(InputAction.CallbackContext context)
        {
            if (passthroughEnabled)
            {
                DisablePassthrough();
            }
            else
            {
                EnablePassthrough();
            }
            passthroughEnabled = !passthroughEnabled;
        }

        public void StartRecording()
        {
            captureController.SetOutputPath(eyetrackingManager.GetSubFolderPath());
            eyetrackingManager.SetUpPath();
            eyetrackingDataRetriever.StartRecording();

            Debug.Log("#Start Recording");
            captureController.StartRecording();
            eyetrackingManager.StartRecording();
        }

        public void StopRecording()
        {
            captureController.EndRecording();
            eyetrackingDataRetriever.StopRecording();
            eyetrackingManager.FinishWriteCSV();

            Debug.Log("#Stop Recording");
        }

        public void EnablePassthrough()
        {
            passthroughController.EnablePassthrough();
        }

        public void DisablePassthrough()
        {
            passthroughController.DisablePassthrough();
        }
    }

}