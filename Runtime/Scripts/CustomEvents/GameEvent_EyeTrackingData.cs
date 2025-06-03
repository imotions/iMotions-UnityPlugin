using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Meta
{

    [CreateAssetMenu(fileName = "GameEvent_EyeTrackingData", menuName = "Event/New GameEvent EyeTrackingData", order = 1)]
    public class GameEvent_EyeTrackingData : ScriptableObject
    {
        private List<GameEventListener_EyeTrackingData> listeners = new List<GameEventListener_EyeTrackingData>();

        public void Raise(EyeTrackingData s)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(s);
            }
        }

        public void RegisterListener(GameEventListener_EyeTrackingData listener)
        {
            listeners.Add(listener);
        }

        public void UnRegisterListener(GameEventListener_EyeTrackingData listener)
        {
            listeners.Remove(listener);
        }
    }
}