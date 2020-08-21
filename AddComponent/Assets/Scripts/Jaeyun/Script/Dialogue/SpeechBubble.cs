using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.Jaeyun.Script.Dialogue
{
    public class SpeechBubble : MonoBehaviour
    {

        public Image bubbleBox;
        public Image portrait;
        public TextMeshProUGUI _textMeshPro;
        
        public void PlaySpeech(SpeechNode node, Action callback)
        {
            portrait.sprite = node.portrait;
            
            node.speakerData.textColor.a = 1;
            _textMeshPro.color = node.speakerData.textColor;
            
            _textMeshPro.text = "";
            StartCoroutine(ShowTextRoutine(node.text, node.textPerDelay, callback));
            
        }

        IEnumerator ShowTextRoutine(string text, float delayPerWord, Action callback)
        {

            bubbleBox.enabled = false;
            yield return new WaitForSeconds(.5f);
            bubbleBox.enabled = true;
            
            var delay = new WaitForSeconds(delayPerWord);
            
            var sb = new StringBuilder();

            int index = 0;
            
            while (index < text.Length)
            {
                sb.Append(text[index]);
                _textMeshPro.text = sb.ToString();
                
                index++;
                bubbleBox.rectTransform.sizeDelta = _textMeshPro.GetPreferredValues();
                yield return delay;
                
            }
            
            callback?.Invoke();
        }

        public void SkipSpeech()
        {
            
        }

        public void CloseSpeech()
        {
            DestroyImmediate(gameObject);
        }
        
    }
}