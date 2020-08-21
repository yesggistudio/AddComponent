using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Development_Tool;

namespace UnityTemplateProjects.Jaeyun.Script.Actor
{
    public class Actor : MonoBehaviour
    {

        public bool isLocked;
        
        private List<ComponentType> _componentTypes = new List<ComponentType>();

        private Material _myMat;

        private Shader _defaultShader;
        private Shader _OutlineShader;
        
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
                _myMat.SetColor("_OutlineColor", Color.magenta);
            }
            else
            {
                _myMat.SetColor("_OutlineColor", Color.cyan);
            }
        }

        public void AddComponent(ComponentType componentType)
        {
            _componentTypes.Add(componentType);
        }
        
        public void RemoveComponent(ComponentType componentType)
        {
            _componentTypes.Remove(componentType);
        }

    }
}