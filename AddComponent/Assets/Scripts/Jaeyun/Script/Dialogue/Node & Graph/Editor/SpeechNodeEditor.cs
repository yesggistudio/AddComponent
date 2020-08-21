using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace UnityTemplateProjects.Jaeyun.Script.Dialogue.Editor
{
    [CustomNodeEditor(typeof(SpeechNode))]
    public class SpeechNodeEditor : NodeEditor
    {
        private NodePort _prevNode;
        private NodePort _nextNode;

        private SerializedProperty _index;
        
        private SerializedProperty _portrait;
        
        private SerializedProperty _speakerData;
        private SerializedProperty _textPerDelay;
        private SerializedProperty _text;

        public override void OnCreate()
        {
            _prevNode = target.GetInputPort("prevNode");
            _nextNode = target.GetOutputPort("nextNode");

            _index = serializedObject.FindProperty("index");

            _portrait = serializedObject.FindProperty("portrait");

            _speakerData = serializedObject.FindProperty("speakerData");
            _textPerDelay = serializedObject.FindProperty("textPerDelay");
            _text = serializedObject.FindProperty("text");
        }

        public override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PortField(GUIContent.none, _prevNode, GUILayout.MinWidth(0));
            GUILayout.Space(-10);

            EditorGUILayout.PropertyField(_portrait);
            
            EditorGUILayout.PropertyField(_speakerData);
            EditorGUILayout.PropertyField(_textPerDelay, GUILayout.MaxWidth(120));
            _text.stringValue = EditorGUILayout.TextField(new GUIContent("Text"), _text.stringValue);
            
            NodeEditorGUILayout.PortField(GUIContent.none, _nextNode, GUILayout.MinWidth(0));
            GUILayout.Space(-10);
            
            serializedObject.ApplyModifiedProperties();
        }

        public override int GetWidth()
        {
            return 400;
        }

        public override Color GetTint()
        {
            if (_speakerData.objectReferenceValue == null) return base.GetTint();

            var speakerData = (SpeakerData) _speakerData.objectReferenceValue;
            Color col = speakerData.nodeColor;
            col.a = 1;
            return col;
        }
    }
}