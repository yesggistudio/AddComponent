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

        public float textEndDelay;

        public Vector2 offset;

        private bool _isFirst = true;

        private SpeechNode _node;
        
        
        public void PlaySpeech(SpeechNode node, Action callback)
        {

            _node = node;
            
            portrait.sprite = node.portrait;
            
            node.speakerData.textColor.a = 1;
            _textMeshPro.color = node.speakerData.textColor;
            bubbleBox.enabled = false;
            
            SetText("");
            StartCoroutine(ShowTextRoutine(node.text, node.textPerDelay, callback));
        }

        IEnumerator MovePortrait(bool isCome, Action callback = null)
        {
            var rootCanvas = portrait.canvas;
            var pixelRect = new Vector2(rootCanvas.pixelRect.width / rootCanvas.scaleFactor, 
                rootCanvas.pixelRect.height / rootCanvas.scaleFactor);
            var rectTransform = portrait.rectTransform;

            var rightPos = new Vector2(pixelRect.x / 2 + rectTransform.sizeDelta.x * 2,
                -pixelRect.y / 2 + rectTransform.sizeDelta.y * .6f);
            
            
            var leftPos = new Vector2(pixelRect.x /2 - rectTransform.sizeDelta.x,
                rightPos.y);
            leftPos += offset;

            float timeCount = 0;
            while (true)
            {
                var t = Mathf.Clamp01(timeCount / 1f);
                if (isCome)
                {
                    rectTransform.localPosition = Vector2.Lerp(rightPos, leftPos, t);
                }
                else
                {
                    rectTransform.localPosition = Vector2.Lerp(leftPos, rightPos, t);
                }

                if (t >= 1) break;
                
                timeCount += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(.5f);
            
            if (!isCome)
            {
                callback?.Invoke();
                DestroyImmediate(gameObject);
            }
        }

        IEnumerator ShowTextRoutine(string text, float delayPerWord, Action callback)
        {
            if (_isFirst)
            {
                yield return StartCoroutine(MovePortrait(true));
                _isFirst = false;
            }
            else
            {
                yield return new WaitForSeconds(.3f);
            }
            bubbleBox.enabled = true;

            text = text.Replace("\\n", "\n");

            var sb = new StringBuilder();

            int index = 0;
            
            while (index < text.Length)
            {

                float timeCount = 0;
                while (timeCount < delayPerWord)
                {
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        SkipSpeech(callback);
                        yield break;
                    }

                    timeCount += Time.deltaTime;
                    yield return null;
                }
                sb.Append(text[index]);
                SetText(sb.ToString());


                index++;


            }

            yield return new WaitForSeconds(textEndDelay);
            callback?.Invoke();

        }

        public void SkipSpeech(Action callback)
        {
            StopAllCoroutines();
            SetText(_node.text);
            StartCoroutine(WaitInput(callback));
        }

        private void SetText(string text)
        {
            _textMeshPro.text = text;
            bubbleBox.rectTransform.sizeDelta = _textMeshPro.GetPreferredValues();
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
            
            
        }

        public void CloseSpeech(Action callback)
        {
            SetText("");
            bubbleBox.enabled = false;
            StartCoroutine(MovePortrait(false, callback));
            
        }
        
    }
}