using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Dialogue;
using UnityTemplateProjects.Jaeyun.Script.Level;

public class JaeyunTest : MonoBehaviour
{
    private Material _myMat;

    private Shader _defaultShader;
    private Shader _OutlineShader;

    public SpeakerData speakerData;
    
    private void Awake()
    {
        _myMat = GetComponent<SpriteRenderer>().material;
        _defaultShader = Shader.Find("Custom/2D Sprite");
        _OutlineShader = Shader.Find("Shader Graphs/2D DrawOutline");
    }

    [ContextMenu("test")]
    public void Test()
    {
        var levelManager = FindObjectOfType<LevelManager>();
        Debug.Log(levelManager.name);
    }

    private void OnMouseEnter()
    {
        _myMat.shader = _OutlineShader;
    }

    private void OnMouseExit()
    {
        _myMat.shader = _defaultShader;
    }
}
