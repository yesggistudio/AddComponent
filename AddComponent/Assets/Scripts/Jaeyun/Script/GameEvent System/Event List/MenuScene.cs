using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Dialogue;

namespace Jaeyun.Script.GameEvent_System.Event_List
{
    public class MenuScene : MonoBehaviour
    {
        public SpeechManager manager;

        public SpeechGraph graph;

        public GameEvent afterDialogue;
        
        private void Start()
        {
            manager.SetSpeechGraph(graph);
            manager.PlayDialogue(afterDialogue);
        }
    }
}