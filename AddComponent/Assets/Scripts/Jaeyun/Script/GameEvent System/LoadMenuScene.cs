using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Jaeyun.Script.Audio;
using UnityTemplateProjects.Jaeyun.Script.Level;

namespace Jaeyun.Script.GameEvent_System
{
    public class LoadMenuScene : MonoBehaviour
    {
        public Level menuLevel;

        private void Start()
        {
            
            SceneManager.LoadScene(menuLevel.name, LoadSceneMode.Additive);

            AudioManager.Instance.PlayBgm(menuLevel.bgm, menuLevel.volume);
            Invoke(nameof(SetActiveScene), .1f);
        }

        private void SetActiveScene()
        {
            var loadedScene = SceneManager.GetSceneByName(menuLevel.name);
            SceneManager.SetActiveScene(loadedScene);
        }

    }
}