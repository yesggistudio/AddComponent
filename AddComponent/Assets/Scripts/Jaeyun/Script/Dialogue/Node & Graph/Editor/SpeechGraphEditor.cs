using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace UnityTemplateProjects.Jaeyun.Script.Dialogue.Editor
{
    [CustomNodeGraphEditor(typeof(SpeechGraph))]
    public class SpeechGraphEditor : NodeGraphEditor
    {
        
        public override void AddContextMenuItems(GenericMenu menu) {
            Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
            var nodeTypes = NodeEditorReflection.nodeTypes.OrderBy(type => GetNodeMenuOrder(type)).ToArray();
            for (int i = 0; i < nodeTypes.Length; i++) {
                Type type = nodeTypes[i];

                //Get node context menu path
                string path = GetNodeMenuName(type);
                if (string.IsNullOrEmpty(path)) continue;

                // Check if user is allowed to add more of given node type
                XNode.Node.DisallowMultipleNodesAttribute disallowAttrib;
                bool disallowed = false;
                if (NodeEditorUtilities.GetAttrib(type, out disallowAttrib)) {
                    int typeCount = target.nodes.Count(x => x.GetType() == type);
                    if (typeCount >= disallowAttrib.max) disallowed = true;
                }

                // Add node entry to context menu
                if (disallowed) menu.AddItem(new GUIContent(path), false, null);
                else menu.AddItem(new GUIContent(path), false, () => {
                    XNode.Node node = CreateNode(type, pos);
                    NodeEditorWindow.current.AutoConnect(node);
                });
            }
            menu.AddSeparator("");
            if (NodeEditorWindow.copyBuffer != null && NodeEditorWindow.copyBuffer.Length > 0) menu.AddItem(new GUIContent("Paste"), false, () => NodeEditorWindow.current.PasteNodes(pos));
            else menu.AddDisabledItem(new GUIContent("Paste"));
            menu.AddItem(new GUIContent("Preferences"), false, () => NodeEditorReflection.OpenPreferences());
            menu.AddItem(new GUIContent("Sort & Rename All Nodes"), false, () => SortAndRenameNodes());
            menu.AddCustomContextMenuItems(target);
        }


        private void SortAndRenameNodes()
        {
            var speechGraph = (SpeechGraph) target;

            var startNode = speechGraph.GetFirstNode();
        
            var nodeEditor = new NodeEditor();

            TraverseAndRenameSpeechNode(startNode);

            startNode = speechGraph.GetFirstNode();
            
            TraverseAndRenameSpeechNode(startNode);
        
            target.nodes.OrderBy(node =>
            {
            
                var speechNode = node as SpeechNode;
                return speechNode.name;
            });
        
            void TraverseAndRenameSpeechNode(SpeechNode currentNode)
            {
                while(currentNode != null)
                {
                    var prevNode = currentNode.GetPrevNode();
                    if (prevNode == null)
                    {
                        currentNode.index = 0;
                    }
                    else
                    {
                        currentNode.index = prevNode.index + 1;
                    }

                    nodeEditor.target = currentNode;
                    nodeEditor.Rename(currentNode.index.ToString());
                    
                    
                    currentNode = currentNode.GetNextNode();
                    
                }
            }

            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public override string GetNodeMenuName(System.Type type)
        {
            if (type.Namespace.Contains("Dialogue"))
                return $"{type.Name}";

            else return null;
        }
    }
}