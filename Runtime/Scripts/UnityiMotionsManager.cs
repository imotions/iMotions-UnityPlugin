using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Coflow.iMotionsPlugin.Vive
{
    public class UnityiMotionsManager : MonoBehaviour
    {
        [Header("Video Capture Resolution Settings:")]
        [SerializeField] private SupportedResolutions resolution = SupportedResolutions._1080;

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
            captureController.SetupResolution(ResolutionUtils.GetResolution(resolution).x, ResolutionUtils.GetResolution(resolution).y);
            eyetrackingDataRetriever.SetupResolution(ResolutionUtils.GetResolution(resolution).x, ResolutionUtils.GetResolution(resolution).y);
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
            eyetrackingManager.SetUpPath();
            captureController.SetOutputPath(eyetrackingManager.GetSubFolderPath(), eyetrackingManager.GetRegisteredTimestamp());
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

public enum SupportedResolutions
{
    _512,
    _720,
    _1080,
    _1440,
    _2048,
    _2160,
    _2880,
    _3072,
    _4096
}

public static class ResolutionUtils
{
    public static Vector2Int GetResolution(SupportedResolutions res)
    {
        switch (res)
        {
            case SupportedResolutions._512: return new Vector2Int(512, 512);
            case SupportedResolutions._720: return new Vector2Int(720, 720);
            case SupportedResolutions._1080: return new Vector2Int(1080, 1080);
            case SupportedResolutions._1440: return new Vector2Int(1440, 1440);
            case SupportedResolutions._2048: return new Vector2Int(2048, 2048);
            case SupportedResolutions._2160: return new Vector2Int(2160, 2160);
            case SupportedResolutions._2880: return new Vector2Int(2880, 2880);
            case SupportedResolutions._3072: return new Vector2Int(3072, 3072);
            case SupportedResolutions._4096: return new Vector2Int(4096, 4096);
            default: return new Vector2Int(1080, 1080); // fallback
        }
    }
}