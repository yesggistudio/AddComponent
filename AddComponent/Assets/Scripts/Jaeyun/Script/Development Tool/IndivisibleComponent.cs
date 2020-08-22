using System;
using UnityEditor;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    public class IndivisibleComponent : MonoBehaviour
    {
        public ComponentType componentType;

        [SerializeField]
        private Actor.Actor attahcedActor;

        private void Update()
        {
            transform.position =attahcedActor.GetIndivisiblePos(this);
        }

        public ComponentType GetComponentType()
        {
            return componentType;
        }

        public void SetColor(Color color)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
        }

        public void AttachToActor(Actor.Actor actor)
        {
            actor.AddIndivisible(this);
            attahcedActor = actor;
            transform.position = actor.GetIndivisiblePos(this);
        }

        public void RemoveFromActor()
        {
            if (attahcedActor != null)
            {
                attahcedActor.RemoveIndivisible(this);
                attahcedActor = null;
            }
        }
        
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(IndivisibleComponent))]
    public class IndivisibleComponentEditor : Editor
    {

        private Vector2 _dragStartPos;
        private bool _isDragging = false;

        private IndivisibleComponent _indivisible;

        private LayerMask actorMask;

        private Actor.Actor _actor;

        private void OnEnable()
        {
            _indivisible = (IndivisibleComponent) target;
            actorMask = LayerMask.GetMask("Default");
        }

        private void OnSceneGUI()
        {
            var guiEvent = Event.current;
            if (guiEvent.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }
            else
            {
                HandleInput(guiEvent);
            }

        }

        void HandleInput(Event guiEvent)
        {
            Vector2 mousePosition = GetMousePosition(guiEvent.mousePosition);

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 &&
                guiEvent.modifiers == EventModifiers.None)
            {
                //Drag Start
                _dragStartPos = mousePosition;
                _isDragging = true;
                Debug.Log("Drag Start");
            }
            else if (guiEvent.type == EventType.MouseDrag && guiEvent.button == 0 &&
                     guiEvent.modifiers == EventModifiers.None)
            {
                if (_isDragging)
                {
                    var delta = mousePosition - _dragStartPos;
                    _indivisible.transform.Translate(delta);
                    Debug.Log("Dragging");
                    FindActor();

                }
            }
            else if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0 &&
                     guiEvent.modifiers == EventModifiers.None)

            {
                _isDragging = false;
                AttachToActor();
                Debug.Log("Drag Done");
            }

        }

        void AttachToActor()
        {
            _indivisible.SetColor(Color.white);
            if (_actor == null)
            {
                _indivisible.RemoveFromActor();
                return;
            }
            _indivisible.AttachToActor(_actor);
            var so = new SerializedObject(_actor.gameObject);
            so.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }

        void FindActor()
        {
            Collider2D[] hitColliders = new Collider2D[3];

            Vector2 transformPosition = _indivisible.transform.position;
            int colliderNums = Physics2D.OverlapCircleNonAlloc(transformPosition, .4f, hitColliders, actorMask);

            float minDist = float.MaxValue;
            Collider2D minCollider = null;
            
            for (int i = 0; i < colliderNums; i++)
            {
                var distFromDrag = Vector2.Distance(hitColliders[i].transform.position, transformPosition);

                if (distFromDrag < minDist)
                {
                    minDist = distFromDrag;
                    minCollider = hitColliders[i];
                }

            }
            
            Actor.Actor actor;
            if (minCollider == null || (actor = minCollider.GetComponent<Actor.Actor>()) == null)
            {
                _actor = null;
                _indivisible.SetColor(Color.white);
                Debug.Log($"Cannot Find Actor");
            }
            else
            {
                if (actor.isLocked)
                {
                    _actor = null;
                    _indivisible.SetColor(Color.red * .6f);
                    Debug.Log($"Find Actor : {actor.name}, but Locked!");
                }
                else
                {
                    Debug.Log($"Find Actor : {actor.name}");
                    _actor = actor;
                    _indivisible.SetColor(Color.cyan * .6f); 
                }
            }
        }


        Vector2 GetMousePosition(Vector3 mousePosition)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(mousePosition);
            float drawHeight = 0;
            float dstToPlane = (drawHeight - mouseRay.origin.z) / mouseRay.direction.z;
            return mouseRay.GetPoint(dstToPlane);
        }
    }
#endif
    
}