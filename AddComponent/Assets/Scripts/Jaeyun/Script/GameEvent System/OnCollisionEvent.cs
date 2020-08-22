using System;
using UnityEngine;

namespace Jaeyun.Script.GameEvent_System
{
    public class OnCollisionEvent : MonoBehaviour
    {
        public GameEvent gameEvent;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameEvent.Raise();
                Destroy(this);
            }
        }
    }
}