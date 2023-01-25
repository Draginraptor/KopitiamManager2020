using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class IStateMachine<T, U> : MonoBehaviour where U : IStateMachine<T, U>
{
    // Allows for custom enums to be used in the Dictionaries, relevant to the specific FSM
    protected abstract Dictionary<T, IState<T, U>> AllStates { get; }

    // Accessor to look at the current state.
    public abstract IState<T, U> CurrentState { get; }

    // Advance to an entry in the dictionary
    public abstract void Advance(T nextState);

    // Is this state a "completion" state. Are we there yet?
    public abstract bool IsComplete();
}