using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    public class ComponentManager : MonoBehaviour
    {

        public static ComponentManager Instance => instance;

        private static ComponentManager instance;
        
        [Serializable]
        public class ComponentUIData
        {
            public ComponentType type;
            public int count;
        }
        
        public Image componentContent;

        public List<ComponentUIData> componentUIDatas = new List<ComponentUIData>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                {
                    DestroyImmediate(gameObject);
                }
            }
        }
        
        
        public void MakeComponents()
        {
            var allbuttons = FindObjectsOfType<ComponentButton>();

            foreach (ComponentButton button in allbuttons)
            {
                DestroyImmediate(button.gameObject);
            }
            
            foreach (var componentUIData in componentUIDatas)
            {
                for (int i = 0; i < componentUIData.count; i++)
                {
                    PrefabUtility.InstantiatePrefab(componentUIData.type.buttonPrefab, componentContent.transform);
                }
            }
        }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(ComponentManager))]
    public class ComponentManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Make Buttons"))
            {
                var manager = (ComponentManager) target;
                manager.MakeComponents();
            }
        }
    }
    #endif
    
}