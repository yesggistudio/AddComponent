using System;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    public class ComponentButton : MonoBehaviour
    {

        public Drag dragSprite;
        
        private Button _button;

        private Drag _drag;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            
        }

        public void BeginDrag()
        {
            if (!_button.interactable) return;
            StartCoroutine(DragComponentRoutine());
        }
        
        public void EndDrag()
        {
            if (!_button.interactable) return;
            
            StopAllCoroutines();
            var actor = _drag.GetActor();
            if (actor != null)
            {
                _button.interactable = false;
            }
            
            DestroyImmediate(_drag.gameObject);
        }

        IEnumerator DragComponentRoutine()
        {
            _drag = Instantiate(dragSprite);
            var cam = Camera.main;
            while (true)
            {
                var mousePosInViewPort = Input.mousePosition;
                var mousePosInWorldPos = cam.ScreenToWorldPoint(mousePosInViewPort);
                mousePosInWorldPos.z = 0;
                _drag.transform.position = mousePosInWorldPos;
                
                yield return null;
            }
        }
        
        
    }
}