using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Jaeyun.Script.Audio;

namespace UnityTemplateProjects.Jaeyun.Script.Level
{
    public class LevelManager : MonoBehaviour
    {

        public Level prevLevel;
        public Level thisLevel;
        public Level nextLevel;
        
        public void LoadPrevLevel()
        {
            StartCoroutine(LoadLevelAsync(prevLevel));
        }

        public void ReLoadLevel()
        {
            StartCoroutine(LoadLevelAsync(thisLevel));
        }
        
        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevelAsync(nextLevel));
        }
        
        private IEnumerator LoadLevelAsync(Level loadLevel)
        {
            
            AudioManager.Instance.StopAllBgm();
            
            var unloadSceneAsync = SceneManager.UnloadSceneAsync(thisLevel.sceneName);
            var loadSceneAsync = SceneManager.LoadSceneAsync(loadLevel.sceneName, LoadSceneMode.Additive);

            while (!(loadSceneAsync.isDone && unloadSceneAsync.isDone))
            {
                yield return null;
            }
            
            AudioManager.Instance.PlayBgm(loadLevel.bgm);
        }
        
    }
}