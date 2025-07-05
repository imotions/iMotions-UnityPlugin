using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Coflow.iMotionsPlugin.Vive
{
    public class EyeTrackingFileManager : MonoBehaviour
    {
        private const string directoryName = "iMotions";
        private const string subDirectoryName = "Study-Files";
        private const string fileName = "EyeTrackingDataFile";

        [Header("Events:")]
        [SerializeField] private GameEvent onFinishedWritingEyeTrackingDataFile;
        [Header("Listeners:")]
        [SerializeField] private GameEventListener_EyeTrackingData onEyetrackingDataSend;

        private List<EyeTrackingData> allEyeTrackingData;

        private bool acceptMoreData;
        private bool startRecording;

        string fullPath;
        string subFolderPath;
        string timestamp;
        const string fileExtension = ".csv";

        const string timestampColumn = "Timestamp";

        const string gazeLeftXColumn = "GazeLeftX";
        const string gazeLeftYColumn = "GazeLeftY";
        const string pupilDialLeftColumn = "PupilDiaLeft";

        const string gazeRightXColumn = "GazeRightX";
        const string gazeRightYColumn = "GazeRightY";
        const string pupilDialRightColumn = "PupilDiaRight";

        const string eyeBlinkingLeftColumn = "EyeBlinkLeft";
        const string eyeBlinkingRightColumn = "EyeBlinkRight";

        //VIVE ONLY
        const string pupilPositionLeftXColumn = "PupilPosLeftX";
        const string pupilPositionLeftYColumn = "PupilPosLeftY";

        const string pupilPositionRightXColumn = "PupilPosRightX";
        const string pupilPositionRightYColumn = "PupilPosRightY";

        const string eyeOpennessLeftColumn = "EyeOpenLeft";
        const string eyeOpennessRightColumn = "EyeOpenRight";

        const string eyeSqueezeLeftColumn = "EyeSqueezeLeft";
        const string eyeSqueezeRightColumn = "EyeSqueezeRight";

        const string eyeWideLeftColumn = "EyeWideLeft";
        const string eyeWideRightColumn = "EyeWideRight";

        const string absoluteTimeColumn = "AbsoluteTime";

        public string GetSubFolderPath()
        {
            return subFolderPath;
        }

        private void Awake()
        {
            acceptMoreData = true;
            allEyeTrackingData = new List<EyeTrackingData>();
            onEyetrackingDataSend.Response.AddListener(EyetrackerAddData);
        }
        public string GetRegisteredTimestamp()
        {
            return timestamp;
        }


        public void SetUpPath()
        {
            timestamp = DateTime.Now.ToString("yyyy-MM-dd-HHmm");
            subFolderPath = Application.persistentDataPath + "/" + directoryName + "/" + subDirectoryName + "-" + timestamp;
            fullPath = subFolderPath + "/" + fileName + "-" + timestamp + fileExtension;

            Directory.CreateDirectory(subFolderPath);

            StreamWriter ki;
            if (!Directory.Exists(fullPath))
            {
                Debug.Log("#data new file start");
                ki = new StreamWriter(fullPath);
                ki.WriteLine($"{timestampColumn}," +
                    $"{gazeLeftXColumn},{gazeLeftYColumn},{pupilDialLeftColumn},{pupilPositionLeftXColumn},{pupilPositionLeftYColumn}," +
                    $"{eyeOpennessLeftColumn},{eyeBlinkingLeftColumn}," +
                    $"{gazeRightXColumn},{gazeRightYColumn},{pupilDialRightColumn},{pupilPositionRightXColumn},{pupilPositionRightYColumn}," +
                    $"{eyeOpennessRightColumn},{eyeBlinkingRightColumn},{absoluteTimeColumn}");

                ki.Close();
            }
        }

        public void StartRecording()
        {
            Debug.Log("#data start writing");
            startRecording = true;
            acceptMoreData = true;

        }
        public void WriteCSV(List<EyeTrackingData> _data)
        {
            StreamWriter ki;
            Debug.Log("PATH :::: " + fullPath);
            ki = new StreamWriter(fullPath, append: true);
            ki.BaseStream.Seek(0, SeekOrigin.End);
            Debug.Log("#data adding data");
            foreach (EyeTrackingData e in _data)
            {
                ki.WriteLine("" + e.timeStamp + "," +
                    e.gazeLeftX + "," + e.gazeLeftY + "," + e.pupilDiaLeft + "," + e.pupilPositionLeftX + "," + e.pupilPositionLeftY + "," +
                    e.eyeOpennessLeft + "," + e.leftEyeBlinking + "," +
                    e.gazeRightX + "," + e.gazeRightY + "," + e.pupilDiaRight + "," + e.pupilPositionRightX + "," + e.pupilPositionRightY + "," +
                    e.eyeOpennessRight + "," + e.rightEyeBlinking + "," + e.absoluteTimestamp);

            }
            ki.Close();
            allEyeTrackingData.RemoveRange(0, _data.Count);
        }

        public void FinishWriteCSV()
        {
            acceptMoreData = false;

            try
            {
                if (allEyeTrackingData.Count > 0)
                {
                    // Use the same format as WriteCSV
                    WriteCSV(new List<EyeTrackingData>(allEyeTrackingData));
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error finishing to write CSV: " + e.Message);
            }

            allEyeTrackingData.Clear();
            startRecording = false;
            onFinishedWritingEyeTrackingDataFile.Raise();
        }

        public void EyetrackerAddData(EyeTrackingData _data)
        {
            if (startRecording)
            {
                if (acceptMoreData)
                {
                    allEyeTrackingData.Add(_data);
                }
            }
        }
    }
}