using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Jaeyun.Script.Audio;

namespace UnityTemplateProjects.Jaeyun.Script.Level
{
    public class LevelManager : MonoBehaviour
    {

        public void LoadPrevLevel()
        {
            var levelData = FindObjectOfType<LevelData>();
            StartCoroutine(LoadLevelAsync(levelData.prevLevel));
        }

        public void ReLoadLevel()
        {
            var levelData = FindObjectOfType<LevelData>();
            StartCoroutine(LoadLevelAsync(levelData.thisLevel));
        }
        
        public void LoadNextLevel()
        {
            var levelData = FindObjectOfType<LevelData>();
            StartCoroutine(LoadLevelAsync(levelData.nextLevel));
        }
        
        private IEnumerator LoadLevelAsync(Level loadLevel)
        {

            var connectDataSaver = FindObjectOfType<ConnectDataSave>();
            connectDataSaver.SaveConnectData();
            
            AudioManager.Instance.StopAllBgm();

            var activeScene = SceneManager.GetActiveScene();
            
            var unloadSceneAsync = SceneManager.UnloadSceneAsync(activeScene);
            var loadSceneAsync = SceneManager.LoadSceneAsync(loadLevel.sceneName, LoadSceneMode.Additive);

            while (!(loadSceneAsync.isDone && unloadSceneAsync.isDone))
            {
                yield return null;
            }

            var loadedScene = SceneManager.GetSceneByName(loadLevel.sceneName);
            SceneManager.SetActiveScene(loadedScene);
            
            AudioManager.Instance.PlayBgm(loadLevel.bgm);

            connectDataSaver.LoadConnectData();
            
            yield return null;
        }
        
    }
}