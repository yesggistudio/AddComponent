using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.Jaeyun.Script.UI
{
    public class UICanvas : MonoBehaviour
    {

        public Image fadeImage;
        public float fadeTime;

        public Coroutine FadeIn()
        {
            return StartCoroutine(FadeRoutine(0));
        }

        public Coroutine FadeOut()
        {
            return StartCoroutine(FadeRoutine(1));
        }

        IEnumerator FadeRoutine(float targetAlpha)
        {
            float startAlpha = fadeImage.color.a;

            float timeCount = 0;
            
            while (true)
            {
                var t = Mathf.Clamp01(timeCount / fadeTime);
                var lerpAlpha = Mathf.Lerp(startAlpha, targetAlpha,t);
                SetAlpha(lerpAlpha);
                
                timeCount += Time.deltaTime;
                
                if (t >= 1) break;
                
                yield return null;
            }
        }

        private void SetAlpha(float alpha)
        {
            var imageColor = fadeImage.color;
            imageColor.a = alpha;
            fadeImage.color = imageColor;
        }
        
    }
}