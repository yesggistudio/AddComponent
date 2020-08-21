using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Level
{
    [CreateAssetMenu(fileName = "new Level", menuName = "Level", order = 0)]
    public class Level : ScriptableObject
    {
        public string sceneName;
        public AudioClip bgm;
    }
}