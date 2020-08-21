using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Development_Tool;

namespace UnityTemplateProjects.Jaeyun.Script.Actor
{
    public class Actor : MonoBehaviour
    {

        public bool isLocked;

        private List<Drag> _drags = new List<Drag>();

        private List<ComponentType> _componentTypes = new List<ComponentType>();

        private Material _myMat;

        private Shader _defaultShader;
        private Shader _OutlineShader;
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

        private void Awake()
        {
            _myMat = GetComponent<SpriteRenderer>().material;
            _defaultShader = Shader.Find("Custom/2D Sprite");
            _OutlineShader = Shader.Find("Shader Graphs/2D DrawOutline");
        }

        public void DrawNormal()
        {
            _myMat.shader = _defaultShader;
        }
        
        public void DrawOutline()
        {
            _myMat.shader = _OutlineShader;
            if (isLocked)
            {
                _myMat.SetColor(OutlineColor, Color.magenta);
            }
            else
            {
                _myMat.SetColor(OutlineColor, Color.cyan);
            }
        }

        public void AddDrag(Drag drag)
        {
            _drags.Add(drag);
        }
        
        public void RemoveDrag(Drag drag)
        {
            _drags.Remove(drag);
            ReSortDrags();
        }

        private void ReSortDrags()
        {
            foreach (var drag in _drags)
            {
                drag.SortUpperActorHead();
            }
        }

        public Vector2 GetComponentPos(Drag drag)
        {
            var result = transform.position;

            var index = _drags.FindIndex(x => x == drag);

            result.y += + (_componentTypes.Count + index + 1) * .6f;

            return result;
        }

    }
}