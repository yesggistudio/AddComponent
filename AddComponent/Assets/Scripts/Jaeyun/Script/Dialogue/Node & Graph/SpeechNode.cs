using UnityEngine;
using XNode;

namespace UnityTemplateProjects.Jaeyun.Script.Dialogue
{
    public class SpeechNode : Node
    {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)]
        public SpeechNode prevNode;
        
        [Output(ShowBackingValue.Never, ConnectionType.Override)]
        public SpeechNode nextNode;

        public int index;
        
        public SpeakerData speakerData;

        public float textPerDelay;

        public Sprite portrait;
        
        public string text;

        public SpeechNode GetPrevNode()
        {
            NodePort port = GetInputPort("prevNode");
            
            if (!isAvailablePort(port))
            {
                return null;
            }
            
            return port.GetConnection(0).node as SpeechNode;
        }
        
        public SpeechNode GetNextNode()
        {
            
            NodePort port = GetOutputPort("nextNode");
            
            if (!isAvailablePort(port))
            {
                return null;
            }
            
            return port.GetConnection(0).node as SpeechNode;
        }

        private bool isAvailablePort(NodePort port)
        {
            return (port != null && port.ConnectionCount > 0);
        }
        
    }
}