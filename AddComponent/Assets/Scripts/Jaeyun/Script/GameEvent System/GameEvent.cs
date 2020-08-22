using System.Collections.Generic;
using UnityEngine;

namespace Jaeyun.Script.GameEvent_System
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent", order = 0)]
    public class GameEvent : ScriptableObject
    {
        public List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise()
        {
            if (listeners.Count > 0)
            {
                for (int i = listeners.Count - 1; i >= 0; i--)
                {
                    listeners[i].OnEventRaised();
                }
            }
        }
	
        public void Raise<T>(T param1)
        {
            if (listeners.Count > 0)
            {
                for (int i = listeners.Count - 1; i >= 0; i--)
                {
                    listeners[i].OnEventRaised(param1);
                }
            }
        }
	
        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }
	
        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}