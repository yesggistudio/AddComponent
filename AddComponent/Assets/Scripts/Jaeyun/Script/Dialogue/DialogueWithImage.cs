using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.Jaeyun.Script.Audio;

namespace UnityTemplateProjects.Jaeyun.Script.Dialogue
{
    public class DialogueWithImage : MonoBehaviour
    {

        public AudioClip audioClip;
        public float endDelay;

        public float fadeTime;
        
        public Image image;
        public TextMeshProUGUI tmp;
        
        public void PlayDialogue(SpeechNode node, Action callback)
        {
            SetAlpha(0);
            image.sprite = node.portrait;
            
            StartCoroutine(DialogueRoutine(node.text, node.textPerDelay, callback));
        }

        IEnumerator DialogueRoutine(string text, float textDelay ,Action callback)
        {
            var delay = new WaitForSeconds(textDelay);
            
            text = text.Replace("\\n", "\n");
            
            yield return StartCoroutine(FadeRoutine(1));
            var sb = new StringBuilder();
            var index = 0;
            while (index < text.Length)
            {
                sb.Append(text[index]);
                tmp.text = sb.ToString();
                
                if(!char.IsWhiteSpace(text[index]))
                    AudioManager.Instance.PlaySfx(audioClip);
                
                index++;
                yield return delay;
            }

            yield return new WaitForSeconds(endDelay);
            tmp.text = "";
            yield return StartCoroutine(FadeRoutine(0));
            callback?.Invoke();
        }

        IEnumerator FadeRoutine(float targetAlpha)
        {

            float timeCount = 0;
            
            var startAlpha = image.color.a;
            
            while (true)
            {
                var t = Mathf.Clamp01(timeCount / fadeTime);
                SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));
                
                if (t >= 1)
                {
                    break;
                }

                timeCount += Time.deltaTime;
                yield return null;
            }
        }

        private void SetAlpha(float alpha)
        {
            var c0 = Color.white;
            c0.a = alpha;
            image.color = c0;
        }
    }
}