using System.Collections;
using System.Collections.Generic;
using Jaeyun.Script.GameEvent_System;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Dialogue;

public class OnTriggerSpeak : MonoBehaviour
{
    public SpeechGraph graph;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var speechManager = FindObjectOfType<SpeechManager>();
            speechManager.SetSpeechGraph(graph);
            speechManager.PlaySpeech(null);
            Destroy(this);
        }
    }
}
