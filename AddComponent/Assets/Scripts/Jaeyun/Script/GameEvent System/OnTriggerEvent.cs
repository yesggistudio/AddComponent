using System;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Actor;

namespace Jaeyun.Script.GameEvent_System
{
    public class OnTriggerEvent : MonoBehaviour
    {
        public GameEvent gameEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Rock")
            {
                other.gameObject.GetComponent<Actor>().DRockFx();
            }

            if (other.gameObject.tag == "Bomb")
            {
                other.gameObject.GetComponent<Actor>().DBombFx();
            }

            if (other.CompareTag("Player"))
            {
                gameEvent.Raise();  
                Destroy(this);
            }
        }
    }
}