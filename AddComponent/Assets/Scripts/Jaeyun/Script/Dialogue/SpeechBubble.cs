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

        private bool _isFirst = true;

        private SpeechNode _node;
        
        
        public void PlaySpeech(SpeechNode node, Action callback)
        {

            _node = node;
            
            portrait.sprite = node.portrait;
            
            node.speakerData.textColor.a = 1;
            _textMeshPro.color = node.speakerData.textColor;
            
            _textMeshPro.text = "";
            StartCoroutine(ShowTextRoutine(node.text, node.textPerDelay, callback));
        }

        IEnumerator MovePortrait()
        {
            var rootCanvas = portrait.canvas;
            var pixelRect = new Vector2(rootCanvas.pixelRect.width / rootCanvas.scaleFactor, 
                rootCanvas.pixelRect.height / rootCanvas.scaleFactor);
            var rectTransform = portrait.rectTransform;

            var startPos = new Vector2(pixelRect.x / 2 + rectTransform.sizeDelta.x * 2,
                -pixelRect.y / 2 + rectTransform.sizeDelta.y * .6f);
            
            var endPos = new Vector2(pixelRect.x /2 - rectTransform.sizeDelta.x,
                startPos.y);

            float timeCount = 0;
            while (true)
            {
                var t = Mathf.Clamp01(timeCount / 1f);
                rectTransform.localPosition = Vector2.Lerp(startPos, endPos, t);

                if (t >= 1) break;
                
                timeCount += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator ShowTextRoutine(string text, float delayPerWord, Action callback)
        {
            if (_isFirst)
            {
                yield return StartCoroutine(MovePortrait());
                _isFirst = false;
            }
            else
            {
                yield return new WaitForSeconds(.3f);
            }

            var delay = new WaitForSeconds(delayPerWord);

            var sb = new StringBuilder();

            int index = 0;
            
            while (index < text.Length)
            {

                yield return null;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SkipSpeech(callback);
                    yield break;
                }
                
                sb.Append(text[index]);
                _textMeshPro.text = sb.ToString();
                
                index++;
                bubbleBox.rectTransform.sizeDelta = _textMeshPro.GetPreferredValues();
                yield return delay;
                
            }
            
            StartCoroutine(WaitInput(callback));

        }

        public void SkipSpeech(Action callback)
        {
            StopAllCoroutines();
            _textMeshPro.text = _node.text;
            _textMeshPro.ForceMeshUpdate();
            bubbleBox.rectTransform.sizeDelta = _textMeshPro.GetPreferredValues();
            StartCoroutine(WaitInput(callback));
        }

        IEnumerator WaitInput(Action callback)
        {
            yield return null;
            
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    break;
                }
                yield return null;
            }
            callback?.Invoke();
        }

        public void CloseSpeech()
        {
            DestroyImmediate(gameObject);
        }
        
    }
}