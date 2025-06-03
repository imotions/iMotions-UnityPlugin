using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Coflow.iMotionsPlugin.Meta
{
    public class GameEventListener_EyeTrackingData : MonoBehaviour
    {
        public GameEvent_EyeTrackingData Event;
        public UnityEvent_EyeTracking Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnRegisterListener(this);
        }

        public void OnEventRaised(EyeTrackingData s)
        {
            Response.Invoke(s);
        }
    }


    [System.Serializable]
    public class UnityEvent_EyeTracking : UnityEvent<EyeTrackingData>
    {
    }
}