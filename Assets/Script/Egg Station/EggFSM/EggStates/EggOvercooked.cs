using System.Collections.Generic;
using UnityEngine;

public class EggOvercooked : IState<EggState, EggFSM>
{
    private EggState _name = EggState.Overcooked;
    private List<ITransition<EggState, EggFSM>> _transitions = new List<ITransition<EggState, EggFSM>>();
    public override List<ITransition<EggState, EggFSM>> Transitions { get { return _transitions; } }

    float _transitionCheckDelayCount = 0f;

    public EggOvercooked(EggFSM fsm)
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
        _fsm.EggBoiledLevel = EggState.Overcooked;
    }

    public override void Run()
    {
        _transitionCheckDelayCount += Time.deltaTime;
        if(_transitionCheckDelayCount > 3f)
        {
            _transitionCheckDelayCount = 0f;
            CheckTransitions();
        }
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new EggOvercookedToEnd(_fsm));
    }
}