using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Coflow.iMotionsPlugin.Meta
{
    public class EyeTrackingFileManager : MonoBehaviour
    {
        private const string directoryName = "iMotions";
        private const string fileName = "EyeTrackingDataFile";

        [Header("Events:")]
        [SerializeField] private GameEvent onFinishedWritingEyeTrackingDataFile;
        [Header("Listeners:")]
        [SerializeField] private GameEventListener_EyeTrackingData onEyetrackingDataSend;

        private List<EyeTrackingData> allEyeTrackingData;

        private bool acceptMoreData;
        private bool startRecording;

        string fullPath;
        const string fileExtension = ".csv";

        public string GetSubFolderPath()
        {
            return Application.persistentDataPath + "/" + directoryName;
        }

        private void Awake()
        {
            acceptMoreData = true;
            allEyeTrackingData = new List<EyeTrackingData>();
            onEyetrackingDataSend.Response.AddListener(EyetrackerAddData);
        }


        public void SetUpPath()
        {
            fullPath = Application.persistentDataPath + "/" + directoryName + "/" + fileName + "-" + DateTime.Now.ToString("dd-MM-yyyy-HHmm") + fileExtension;

            Directory.CreateDirectory(Application.persistentDataPath + "/" + directoryName);

            StreamWriter ki;
            if (!Directory.Exists(fullPath))
            {
                Debug.Log("#data new file start");
                ki = new StreamWriter(fullPath);
                ki.WriteLine("Timestamp,GazeLeftX,GazeLeftY,PupilDiaLeft,GazeRightX,GazeRightY,PupilDiaRight");
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
                ki.WriteLine("" + e.timeStamp + "," + e.gazeLeftX + "," + e.gazeLeftY + "," + e.pupilDiaLeft + "," + e.gazeRightX + "," + e.gazeRightY + "," + e.pupilDiaRight);
            }
            ki.Close();
            allEyeTrackingData.RemoveRange(0, _data.Count);
        }

        public void FinishWriteCSV()
        {
            Debug.Log("#data End data start writing");
            acceptMoreData = false;

            try
            {
                StreamWriter ki = new StreamWriter(fullPath, append: true);
                ki.BaseStream.Seek(0, SeekOrigin.End);
                foreach (EyeTrackingData e in allEyeTrackingData)
                {
                    ki.WriteLine("" + e.timeStamp + "," + e.gazeLeftX + "," + e.gazeLeftY + "," + e.pupilDiaLeft + "," + e.gazeRightX + "," + e.gazeRightY + "," + e.pupilDiaRight);
                }
                ki.Close();
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