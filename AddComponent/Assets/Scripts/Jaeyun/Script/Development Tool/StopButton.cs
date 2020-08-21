using System;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.Jaeyun.Script.Level;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    public class StopButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            var levelManager = FindObjectOfType<LevelManager>();
            _button.onClick.AddListener(() => levelManager.ReLoadLevel());
            
        }
    }
}