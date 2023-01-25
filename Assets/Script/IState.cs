using System.Collections.Generic;
using UnityEngine;

public abstract class IState<T, U> where U: IStateMachine<T, U>
{
    protected U _fsm;

    public abstract List<ITransition<T, U>> Transitions { get; }

    // Utility function to help display useful things
    public abstract string GetName();

    // By default does nothing
    public virtual void Enter()
    {
        return;
    }

    // Default is to simply check for when to transition
    public virtual void Run()
    {
        CheckTransitions();
    }

    // By default does nothing
    public virtual void Exit()
    {
        return;
    }

    protected virtual void CheckTransitions()
    {
        for(int i = 0; i < Transitions.Count; i++)
        {
            // Run the check on the transition
            if(Transitions[i].TransitionCheck())
            {
                // On true
                _fsm.Advance(Transitions[i].TargetState);
            }
        }
    }

    // This isn't really needed, but it helps in debugging and other tasks.
    // It allows for hover-tips and debug info to show me the name of the state
    // rather than the default of the type of the object
    public override string ToString()
    {
        return GetName();
    }

    public abstract void SetupTransitions();
}