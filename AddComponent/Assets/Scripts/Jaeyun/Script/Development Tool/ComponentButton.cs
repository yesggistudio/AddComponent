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

        public ComponentType componentType;
        
        public Drag dragSprite;
        
        private Button _button;

        private Drag _drag;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            
        }

        public void SetInteractable(bool value)
        {
            _button.interactable = value;
        }

        public void BeginDrag()
        {
            if (!_button.interactable) return;
            
            _drag = Instantiate(dragSprite, _button.image.canvas.transform);
            _drag.InitializeDrag(this, _button.image.sprite);

        }
        
        public void EndDrag()
        {
            if (!_button.interactable) return;

            if (_drag.Attach())
            {
                _button.interactable = false;
            }
            else
            {
                DestroyDrag();
            }
        }

        public void DestroyDrag()
        {
            _button.interactable = true;
            DestroyImmediate(_drag.gameObject);
            _drag = null;
        }

        
        
    }
}