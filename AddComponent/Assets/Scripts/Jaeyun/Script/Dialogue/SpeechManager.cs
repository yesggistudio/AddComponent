using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Dialogue;

public class SpeechManager : MonoBehaviour
{
    public SpeechBubble bubblePrefab;

    public SpeechNode test;

    [ContextMenu("Test")]
    private void Test()
    {
        PlaySpeech(test);
    }

    public SpeechBubble PlaySpeech(SpeechNode node)
    {
        var bubble = Instantiate(bubblePrefab, transform);
        bubble.PlaySpeech(node, () => {Debug.Log("Done"); });
        return bubble;
    }
    
    
}
