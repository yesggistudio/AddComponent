using System;
using System.Collections;
using System.Collections.Generic;
using Jaeyun.Script.GameEvent_System;
using UnityEngine;

public class DeadPannel : MonoBehaviour
{
    public GameEvent gameEvent;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    [ContextMenu("Test")]
    public void DoDead()
    {
        StartCoroutine(DeadRoutine());
    }
    
    IEnumerator DeadRoutine()
    {
        yield return StartCoroutine(ColorChange(Color.black, .06f));
        yield return StartCoroutine(ColorChange(Color.white,.06f));
        yield return StartCoroutine(ColorChange(Color.black, .1f));
        gameEvent.Raise();
    }

    IEnumerator ColorChange(Color color, float duration)
    {
        var currentColor = _spriteRenderer.color;
        
        float timeCount = 0;
        while (true)
        {
            var t = Mathf.Clamp01(timeCount / duration);
            var lerpColor = Color.Lerp(currentColor, color, t);
            _spriteRenderer.color = lerpColor;
            timeCount += Time.deltaTime;
            if (t >= 1) break;
            yield return null;
        }
    }
    
}
