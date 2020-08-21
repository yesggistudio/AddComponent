using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Dialogue;

public class SpeechManager : MonoBehaviour
{
    public SpeechBubble bubblePrefab;

    public SpeechGraph test;

    [ContextMenu("Test")]
    private void Test()
    {
        PlaySpeech(test);
    }

    public SpeechBubble PlaySpeech(SpeechGraph speechGraph)
    {
        var node = speechGraph.GetFirstNode();
        var bubble = Instantiate(bubblePrefab, transform);

        void PlayNextNode(SpeechNode speechNode)
        {
            var nextNode = speechNode.GetNextNode();
            if (nextNode == null)
            {
                bubble.CloseSpeech();
            }
            else
            {
                bubble.PlaySpeech(nextNode, () => PlayNextNode(nextNode));
            }
        }
        
        
        bubble.PlaySpeech(node, () => PlayNextNode(node));
        return bubble;
    }
    
    
}
