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

        public bool isMake;
        public class MakeButtonsInfos
        {
            public ComponentType componentType;
            public float makeMinTime;
            public float makeMaxTime;
        }

        public List<MakeButtonsInfos> makeButtonsInfoses = new List<MakeButtonsInfos>();

        public void StartMake()
        {
            foreach (var makeButtonsInfose in makeButtonsInfoses)
            {
                var buttonPrefab = makeButtonsInfose.componentType.buttonPrefab;
                StartCoroutine(MakeButtonRoutine(makeButtonsInfose.makeMinTime, makeButtonsInfose.makeMaxTime, buttonPrefab));
            }
        }

        public void StopMake()
        {
            StopAllCoroutines();
        }

        IEnumerator MakeButtonRoutine(float minTime, float maxTime, ComponentButton buttonPrefab)
        {
            while (true)
            {
                var randomTime = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(randomTime);

                
                //이 부분 오브젝트 풀링 써주시는게 좋을듯?
                Instantiate(buttonPrefab);
            }
        }
        
    }
}