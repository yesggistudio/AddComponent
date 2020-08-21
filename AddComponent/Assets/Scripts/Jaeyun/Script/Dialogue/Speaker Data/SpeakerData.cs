using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Dialogue
{
    [CreateAssetMenu(fileName = "New SpeakerData", menuName = "Speech/SpeakerData", order = 0)]
    public class SpeakerData : ScriptableObject
    {
        public Color textColor;
        public Color nodeColor;
        
    }
}