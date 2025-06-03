using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Vive
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Event/New GameEvent", order = 1)]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnRegisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}