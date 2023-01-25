using System.Collections.Generic;
using UnityEngine;

public class EggSoftBoiled : IState<EggState, EggFSM>
{
    private EggState _name = EggState.SoftBoiled;
    private List<ITransition<EggState, EggFSM>> _transitions = new List<ITransition<EggState, EggFSM>>();
    public override List<ITransition<EggState, EggFSM>> Transitions { get { return _transitions; } }

    public EggSoftBoiled(EggFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void Enter()
    {
        _fsm.EggBoiledLevel = EggState.SoftBoiled;
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new EggBoiledToReady(_fsm));
    }
}