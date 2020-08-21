using System;
using System.Collections;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    public class Drag : MonoBehaviour
    {
        
        private Actor.Actor _actor;

        public LayerMask actorMask;

        public Actor.Actor GetActor()
        {
            return _actor;
        }

        private void OnEnable()
        {
            StartCoroutine(CheckActor());
        }

        IEnumerator CheckActor()
        {

            var checkDelay = new WaitForSeconds(.1f);
            
            Collider2D[] hitColliders = new Collider2D[5];
            
            while (true)
            {
                int colliderNums = Physics2D.OverlapCircleNonAlloc(transform.position, .2f, hitColliders, actorMask);
                
                float minDist = float.MaxValue;
                Collider2D minCollider = null;
                
                for (int i = 0; i < colliderNums; i++)
                {
                    var distFromDrag = Vector2.Distance(hitColliders[i].transform.position, transform.position);
                    if (distFromDrag < minDist)
                    {
                        minDist = distFromDrag;
                        minCollider = hitColliders[i];
                    }
                }


                Actor.Actor actor;
                if (minCollider == null || (actor = minCollider.GetComponent<Actor.Actor>()) == null)
                {
                    if (_actor != null)
                    {
                        _actor.DrawNormal();
                        _actor = null;
                    }
                }
                
                else
                {
                    
                    var newActor = actor.GetComponent<Actor.Actor>();
                    if (_actor != newActor)
                    {
                        _actor?.DrawNormal();
                        _actor = newActor;
                        _actor.DrawOutline(); 
                    }
                       
                }

                yield return checkDelay;
            }
        }


        private void OnDisable()
        {
            if (_actor != null)
            {
                _actor.DrawNormal();
            }
        }
    }
}