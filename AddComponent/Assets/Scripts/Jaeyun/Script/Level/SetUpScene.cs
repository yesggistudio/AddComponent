using System;
using Jaeyun.Script.GameEvent_System;
using UnityEditor;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Level
{
    public class SetUpScene : MonoBehaviour
    {
        public GameEvent gameEvent;
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(SetUpScene))]
    public class SetUpSceneEditor : Editor
    {
        private SetUpScene _setUpScene;

        private void OnEnable()
        {
            _setUpScene = (SetUpScene) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set Up Scene"))
            {
                _setUpScene.gameEvent.Raise();
            }
        }
    }
    #endif
    
    
    
}