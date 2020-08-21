using UnityEditor;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Editor
{

    public class GenericSelectablePropertyDrawer<T> : PropertyDrawer where T : ScriptableObject
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            // Store old indent level and set it to 0, the PrefixLabel takes care of it

            position = EditorGUI.PrefixLabel(position, label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect buttonRect = position;
            buttonRect.width = 80;

            string buttonLabel = "Select";
            T currentSelectedProperty = property.objectReferenceValue as T;
            if (currentSelectedProperty != null) buttonLabel = currentSelectedProperty.name;
            if (GUI.Button(buttonRect, buttonLabel))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("None"), currentSelectedProperty == null,
                    () => SelectPropertyInfo(property, null));
                string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
                for (int i = 0; i < guids.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    T loadedProperty = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
                    if (loadedProperty != null)
                    {
                        GUIContent content = new GUIContent(loadedProperty.name);
                        string[] nameParts = loadedProperty.name.Split(' ');
                        if (nameParts.Length > 1)
                            content.text = nameParts[0] + "/" + loadedProperty.name.Substring(nameParts[0].Length + 1);
                        menu.AddItem(content, loadedProperty == currentSelectedProperty,
                            () => SelectPropertyInfo(property, loadedProperty));
                    }
                }

                menu.ShowAsContext();
            }

            position.x += buttonRect.width + 4;
            position.width -= buttonRect.width + 4;
            EditorGUI.ObjectField(position, property, typeof(T), GUIContent.none);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private void SelectPropertyInfo(SerializedProperty property, T charInfo)
        {
            property.objectReferenceValue = charInfo;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }
    }
}