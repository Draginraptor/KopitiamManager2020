using System.Collections.Generic;
using UnityEngine;

public class EggHardBoiled : IState<EggState, EggFSM>
{
    private EggState _name = EggState.HardBoiled;
    private List<ITransition<EggState, EggFSM>> _transitions = new List<ITransition<EggState, EggFSM>>();
    public override List<ITransition<EggState, EggFSM>> Transitions { get { return _transitions; } }

    public EggHardBoiled(EggFSM fsm)
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
        _fsm.EggBoiledLevel = EggState.HardBoiled;
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new EggBoiledToReady(_fsm));
    }
}