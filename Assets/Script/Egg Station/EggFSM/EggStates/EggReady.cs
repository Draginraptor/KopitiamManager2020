using System.Collections.Generic;
using UnityEngine;

public class EggReady : IState<EggState, EggFSM>
{
    private EggState _name = EggState.Ready;
    private List<ITransition<EggState, EggFSM>> _transitions = new List<ITransition<EggState, EggFSM>>();
    public override List<ITransition<EggState, EggFSM>> Transitions { get { return _transitions; } }

    public EggReady(EggFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new EggReadyToEnd(_fsm));
    }
}