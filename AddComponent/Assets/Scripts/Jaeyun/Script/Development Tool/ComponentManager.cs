using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    public class ComponentManager : MonoBehaviour
    {
        [Serializable]
        public class ComponentUIData
        {
            public ComponentType type;
            public ComponentButton componentButton;
        }
        
        public Image componentContent;

        public List<ComponentUIData> componentUIDatas = new List<ComponentUIData>();

        public List<ComponentType> usingComponents = new List<ComponentType>();

        [ContextMenu("Make components")]
        private void MakeComponents()
        {
            foreach (var component in usingComponents)
            {
                Instantiate(componentUIDatas.Find(x => x.type == component).componentButton,
                    componentContent.rectTransform);
            }
        }
    }
}