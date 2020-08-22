using System;
using System.Collections;
using System.Collections.Generic;
using IndieMarc.Platformer;
using Jaeyun.Script.GameEvent_System;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Actor;
using UnityTemplateProjects.Jaeyun.Script.Dialogue;

public class SpeechManager : MonoBehaviour
{
    public SpeechBubble bubblePrefab;
    public DialogueWithImage dialogueWithImage;

    public TheGame theGame;
    
    private SpeechGraph _speechGraph;


    public void SetSpeechGraph(SpeechGraph graph)
    {
        _speechGraph = graph;
    }
    
    public void PlaySpeech(GameEvent gameEvent)
    {
        var node = _speechGraph.GetFirstNode();
        var bubble = Instantiate(bubblePrefab, transform);
        theGame?.Pause();
        void PlayNextNode(SpeechNode speechNode)
        {
            var nextNode = speechNode.GetNextNode();
            if (nextNode == null)
            {
                bubble.CloseSpeech(() =>
                {
                    gameEvent?.Raise();
                    theGame?.Unpause();
                });
            }
            else
            {
                bubble.PlaySpeech(nextNode, () => PlayNextNode(nextNode));
            }
        }
        
        
        bubble.PlaySpeech(node, () => PlayNextNode(node));
        
    }
    
    public void PlayDialogue(GameEvent gameEvent)
    {
        
        var node = _speechGraph.GetFirstNode();
        
        void PlayNextNode(SpeechNode speechNode)
        {
            var nextNode = speechNode.GetNextNode();
            if (nextNode == null)
            {
                gameEvent?.Raise();
            }
            else
            {
                dialogueWithImage.PlayDialogue(nextNode, () => PlayNextNode(nextNode));
            }
        }
        
        
        dialogueWithImage.PlayDialogue(node, () => PlayNextNode(node));
    }
    
    
}
