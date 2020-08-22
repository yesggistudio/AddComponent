using System;
using Jaeyun.Script.GameEvent_System;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.UI
{
    public class Title : MonoBehaviour
    {
        public float delayTime;
        public GameEvent gameEvent;
        private void Start()
        {
            Invoke(nameof(InvokeEvent), delayTime);
        }

        public void InvokeEvent()
        {
            gameEvent?.Raise();
        }
    }
}