using System;
using System.Collections.Generic;
using System.Linq;
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
        
        

        [ContextMenu("Make components")]
        private void MakeComponents()
        {
            foreach (var componentUIData in componentUIDatas)
            {
                for (int i = 0; i < componentUIData.count; i++)
                {
                    Instantiate(componentUIData.type.buttonPrefab, componentContent.transform);
                }
            }
        }
    }
}