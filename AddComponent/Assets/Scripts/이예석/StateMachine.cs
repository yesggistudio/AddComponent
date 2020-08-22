using System;
using UnityEngine;

public struct StateChangeEvent<T>where T: struct,IComparable, IConvertible, IFormattable
{
    public GameObject Target;
    public StateMachine<T> TargetStateMachine;
    public T NewState;
    public T PreviousState;

    public StateChangeEvent(StateMachine<T> stateMachine)
    {
        Target = stateMachine.Target;
        TargetStateMachine = stateMachine;
        NewState = stateMachine.CurrentState;
        PreviousState = stateMachine.PreviousState;
    }

    
}
public interface IStateMachine
{
    bool TriggerEvents { get; set; }

}

public class StateMachine<T> : IStateMachine where T : struct, IComparable, IConvertible, IFormattable
{
    public bool TriggerEvents { get; set; }
    public GameObject Target;
    public T CurrentState { get; protected set; }
    public T PreviousState { get; protected set; }

    public StateMachine(GameObject target, bool triggerEvents)
    {
        this.Target = target;
        this.TriggerEvents = triggerEvents;
    }

    public virtual void ChangeState(T newState)
    {
        if (newState.Equals(CurrentState))
        {
            return;
        }

        PreviousState = CurrentState;
        CurrentState = newState;


        //트리거 이벤트

    }

    public virtual void RestorePreviousState()
    {
        CurrentState = PreviousState;


        //트리거 이벤트
    }

}