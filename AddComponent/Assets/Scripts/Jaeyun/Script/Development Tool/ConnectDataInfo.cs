using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    public class ConnectDataInfo : MonoBehaviour
    {
        public List<ComponentButton> componentButtons;
        public List<Actor.Actor> actors;

        public int GetIndex<T>(T target)
        {
            if (target is ComponentButton)
            {
                var button = target as ComponentButton;
                return componentButtons.FindIndex(x => x == button);
            }
            if (target is Actor.Actor)
            {
                var button = target as Actor.Actor;
                return actors.FindIndex(x => x == button);
            }
            throw new Exception("error type");
        }

        public ComponentButton GetButton(int index)
        {
            return componentButtons[index];
        }
        
        public Actor.Actor GetActor(int index)
        {
            return actors[index];
        }

        public void SetUpAllInfos()
        {
            var allButtons = FindObjectsOfType<ComponentButton>();
            componentButtons = allButtons.ToList();

            var allActors = FindObjectsOfType<Actor.Actor>();
            actors = allActors.ToList();
        }
        
    }
    
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(ConnectDataInfo))]
    public class ConnectDataInfoEditor : Editor
    {
        private ConnectDataInfo _connectDataInfo;

        private void OnEnable()
        {
            _connectDataInfo = (ConnectDataInfo) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set Up Data"))
            {
                _connectDataInfo.SetUpAllInfos();
            }
        }
    } 
    #endif
}