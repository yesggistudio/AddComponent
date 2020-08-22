using System;
using Jaeyun.Script.GameEvent_System;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script
{
    [RequireComponent(typeof(Collider2D))]
    public class ColliderCheck : MonoBehaviour
    {
        public GameEvent targetEvent;
        public string targetTag;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(targetTag))
            {
                targetEvent.Raise();
            }       
        }
        
    }
}