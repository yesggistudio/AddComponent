using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Development_Tool;
using Random = UnityEngine.Random;

namespace UnityTemplateProjects.Jaeyun.Script
{
    public class MakeButton : MonoBehaviour
    {
        

        public List<ComponentType> componentTypes = new List<ComponentType>();

        [ContextMenu("Test Start")]
        public void StartMake()
        {
            foreach (var componentType in componentTypes)
            {
                var buttonPrefab = componentType.buttonPrefab;
                StartCoroutine(MakeButtonRoutine(buttonPrefab));
            }
        }

        public void StopMake()
        {
            StopAllCoroutines();
        }

        IEnumerator MakeButtonRoutine(ComponentButton buttonPrefab)
        {
            ComponentButton button = Instantiate(buttonPrefab, transform);
            while (true)
            {
                yield return new WaitWhile(() => button != null);
                 button = Instantiate(buttonPrefab, transform);
            }

        }
        
    }
}