using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Jaeyun.Script.Audio;
using UnityTemplateProjects.Jaeyun.Script.UI;

namespace UnityTemplateProjects.Jaeyun.Script.Level
{
    public class LevelManager : MonoBehaviour
    {

        public UICanvas uiCanvas;
        
        public void LoadPrevLevel()
        {
            var levelData = FindObjectOfType<LevelData>();
            StartCoroutine(LoadLevelAsync(levelData.prevLevel));
        }

        public void ReLoadLevel()
        {
            var levelData = FindObjectOfType<LevelData>();
            StartCoroutine(LoadLevelAsync(levelData.thisLevel, true));
        }
        
        public void LoadNextLevel()
        {
            var levelData = FindObjectOfType<LevelData>();
            StartCoroutine(LoadLevelAsync(levelData.nextLevel));
        }

        public void ClearStage()
        {
            var levelData = FindObjectOfType<LevelData>();
            StartCoroutine(LoadLevelAsync(levelData.nextLevel, false, 1));
        }
        
        private IEnumerator LoadLevelAsync(Level loadLevel, bool isReLoad = false, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            yield return uiCanvas.FadeOut();
            
            var connectDataSaver = FindObjectOfType<ConnectDataSave>();
            if (isReLoad)
            {
                connectDataSaver.SaveConnectData();
            }

            AudioManager.Instance.StopAllBgm();

            var activeScene = SceneManager.GetActiveScene();
            
            var unloadSceneAsync = SceneManager.UnloadSceneAsync(activeScene);
            var loadSceneAsync = SceneManager.LoadSceneAsync(loadLevel.name, LoadSceneMode.Additive);

            while (!(loadSceneAsync.isDone && unloadSceneAsync.isDone))
            {
                yield return null;
            }

            var loadedScene = SceneManager.GetSceneByName(loadLevel.name);
            SceneManager.SetActiveScene(loadedScene);
            
            yield return uiCanvas.FadeIn();
            
            AudioManager.Instance.PlayBgm(loadLevel.bgm, loadLevel.volume);

            if (isReLoad)
            {
                connectDataSaver.LoadConnectData();
            }

            yield return null;
        }

        public void GameOver()
        {
               
            
            //대사 출력
            // var speechManager = GetComponent<SpeechManager>();
            // 대사 파일을 담은 speech graph 넣어주기
            // speechManager.SetSpeechGraph();
            // SpeechGraph 안의 대사를 쭉 보여주기
            // speechManager.PlaySpeech();
            
            //게임 오버시 연출
        }
        
    }
}