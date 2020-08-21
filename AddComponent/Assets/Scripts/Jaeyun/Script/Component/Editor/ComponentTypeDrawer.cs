using UnityEditor;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Editor;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool.Editor
{
    [CustomPropertyDrawer(typeof(ComponentType))]
    public class ComponentTypeDrawer : GenericSelectablePropertyDrawer<ComponentType>
    {
        
    }
}