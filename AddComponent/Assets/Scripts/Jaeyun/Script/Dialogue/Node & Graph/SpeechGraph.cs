using System.Linq;
using UnityEngine;
using XNode;

namespace UnityTemplateProjects.Jaeyun.Script.Dialogue
{
    [CreateAssetMenu(fileName = "New speechGraph", menuName = "Speech/Speech Graph", order = 0)]
    public class SpeechGraph : NodeGraph
    {

        public SpeechNode GetFirstNode()
        {
            return nodes.OfType<SpeechNode>().ToList().OrderBy(x => x.index).ToList()[0];
        }
        
    }
}