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
    //
    // public void DoDead()
    // {
    //     DeadRoutine();
    // }
    //
    // IEnumerator DeadRoutine()
    // {
    //     
    // }
    
}
