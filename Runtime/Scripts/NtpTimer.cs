using System;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Meta
{

    public class NtpTimer : MonoBehaviour
    {
        private DateTime _ntpStartTime;
        private DateTime _localStartTime;
        private bool _ntpTimeReceived = false;
        private NtpClient _ntpClient;

        private string _ntpServer = "pool.ntp.org";

        void Start()
        {
            // Initialize NTP client
            _ntpClient = new NtpClient(_ntpServer);

            // Get NTP time at startup
            GetInitialNtpTime();
        }

        private void GetInitialNtpTime()
        {
            try
            {
                Debug.Log("Retrieving NTP time...");

                // Get the initial NTP time
                _ntpStartTime = _ntpClient.GetNetworkTime();
                _localStartTime = DateTime.UtcNow;
                _ntpTimeReceived = true;

                Debug.Log($"NTP time retrieved: {_ntpStartTime:yyyy-MM-dd HH:mm:ss.fff} UTC");
                Debug.Log($"Local time at start: {_localStartTime:yyyy-MM-dd HH:mm:ss.fff} UTC");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to retrieve NTP time: {ex.Message}");
                Debug.LogError("Using local time as fallback");

                // Fallback to local time if NTP fails
                _ntpStartTime = DateTime.UtcNow;
                _localStartTime = _ntpStartTime;
                _ntpTimeReceived = true;
            }
        }

        /// <summary>
        /// Gets the elapsed milliseconds since the NTP timestamp was retrieved at startup
        /// </summary>
        /// <returns>Elapsed milliseconds as long</returns>
        public long GetElapsedMilliseconds()
        {
            if (!_ntpTimeReceived)
            {
                Debug.LogWarning("NTP time not yet received");
                return 0;
            }

            // Calculate elapsed time using local time (more accurate for duration measurement)
            TimeSpan elapsed = DateTime.UtcNow - _localStartTime;
            return (long)elapsed.TotalMilliseconds;
        }

        /// <summary>
        /// Gets the current NTP-synchronized time
        /// </summary>
        /// <returns>Current estimated NTP time</returns>
        public DateTime GetCurrentNtpTime()
        {
            if (!_ntpTimeReceived)
            {
                Debug.LogWarning("NTP time not yet received");
                return DateTime.UtcNow;
            }

            // Calculate current NTP time by adding elapsed local time to initial NTP time
            TimeSpan elapsed = DateTime.UtcNow - _localStartTime;
            return _ntpStartTime + elapsed;
        }

        /// <summary>
        /// Gets the initial NTP timestamp that was retrieved at startup
        /// </summary>
        /// <returns>Initial NTP timestamp</returns>
        public DateTime GetInitialNtpTimestamp()
        {
            return _ntpStartTime;
        }

        /// <summary>
        /// Checks if NTP time has been successfully retrieved
        /// </summary>
        /// <returns>True if NTP time is available</returns>
        public bool IsNtpTimeAvailable()
        {
            return _ntpTimeReceived;
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Debug.Log($"App paused. Total elapsed time: {GetElapsedMilliseconds()} ms");
            }
            else
            {
                Debug.Log($"App resumed. Total elapsed time: {GetElapsedMilliseconds()} ms");
            }
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                Debug.Log($"App lost focus. Total elapsed time: {GetElapsedMilliseconds()} ms");
            }
            else
            {
                Debug.Log($"App gained focus. Total elapsed time: {GetElapsedMilliseconds()} ms");
            }
        }

        void OnApplicationQuit()
        {
            long finalElapsedMs = GetElapsedMilliseconds();
            Debug.Log($"Application closing. Total elapsed time: {finalElapsedMs} ms");

            // Log final statistics
            if (_ntpTimeReceived)
            {
                Debug.Log($"Session started at: {_ntpStartTime:yyyy-MM-dd HH:mm:ss.fff} UTC");
                Debug.Log($"Session ended at: {GetCurrentNtpTime():yyyy-MM-dd HH:mm:ss.fff} UTC");
                Debug.Log($"Total session duration: {finalElapsedMs} ms ({finalElapsedMs / 1000.0:F2} seconds)");
            }
        }

        void OnDestroy()
        {
            // Clean up NTP client
            _ntpClient?.Dispose();
        }
    }
}
