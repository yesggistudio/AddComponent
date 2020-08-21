using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    public class Drag : MonoBehaviour
    {

        private ComponentButton _componentButton;
        
        private Actor.Actor _actor;
        private RectTransform _rectTransform;

        public LayerMask actorMask;

        private Button _button;

        private Camera _cam;

        public void InitializeDrag(ComponentButton componentButton)
        {
            _button = GetComponent<Button>();
            
            _rectTransform = GetComponent<RectTransform>();
            
            _componentButton = componentButton;
            _button.image.sprite = componentButton.GetSprite();
            
            _cam = Camera.main;
            
        }

        public ComponentButton GetButton()
        {
            return _componentButton;
        }

        public Actor.Actor GetActor()
        {
            return _actor;
        }

        public void BeginDrag()
        {
            _actor?.RemoveDrag(this);
            
            StartCoroutine(ChaseMouse());
            StartCoroutine(CheckActor());
        }

        public void EndDrag()
        {
            if (!Attach())
            {
                _componentButton.DestroyDrag();
            }
        }

        public bool Attach()
        {
            _actor?.DrawNormal();

            var allDrags = FindObjectsOfType<Drag>();
            
            if (_actor != null && !_actor.isLocked)
            {
                _actor.AddDrag(this);
                SortUpperActorHead();
                StopAllCoroutines();
                return true;
            }
            else
            {
                return false;
            }
            
        }
        
        public void LinkToActor(Actor.Actor actor)
        {
            _actor = actor;
            actor.AddDrag(this);
            SortUpperActorHead();
        }

        public void SortUpperActorHead()
        {
            var targetPos = _actor.GetComponentPos(this);
            _rectTransform.position = _cam.WorldToScreenPoint(targetPos);
        }

        IEnumerator ChaseMouse()
        {
            while (true)
            {
                var mousePosInScreenPort = Input.mousePosition;
                _rectTransform.position = mousePosInScreenPort;
                yield return null;
            }
        }

        IEnumerator CheckActor()
        {
            _actor = null;
            
            var checkDelay = new WaitForSeconds(.1f);
            
            Collider2D[] hitColliders = new Collider2D[3];

            var worldPos = Vector2.zero;
            
            while (true)
            {
                worldPos = _cam.ScreenToWorldPoint(_rectTransform.position);

                int colliderNums = Physics2D.OverlapCircleNonAlloc(worldPos, .2f, hitColliders, actorMask);
                
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