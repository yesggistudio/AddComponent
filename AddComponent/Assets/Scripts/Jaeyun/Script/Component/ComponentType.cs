using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    [CreateAssetMenu(fileName = "Component", menuName = "New Component", order = 0)]
    public abstract class ComponentType : ScriptableObject
    {
        public ComponentButton buttonPrefab;

    }
}