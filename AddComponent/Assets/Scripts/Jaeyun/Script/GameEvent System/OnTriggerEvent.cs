using System;
using UnityEngine;

namespace Jaeyun.Script.GameEvent_System
{
    public class OnTriggerEvent : MonoBehaviour
    {
        public GameEvent gameEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                gameEvent.Raise();
                Destroy(this);
            }
        }
    }
}