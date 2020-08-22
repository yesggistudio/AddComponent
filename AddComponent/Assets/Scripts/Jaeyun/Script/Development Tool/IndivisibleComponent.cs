using System;
using UnityEditor;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    public class IndivisibleComponent : MonoBehaviour
    {
        public ComponentType componentType;
        
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(IndivisibleComponent))]
    public class IndivisibleComponentEditor : Editor
    {
        private void OnSceneGUI()
        {
            
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