using UnityEngine;

public class EggCookingToHalfboiled : ITransition<EggState, EggFSM>
{
    private EggState _targetState = EggState.HalfBoiled;
    public override EggState TargetState { get { return _targetState; } }

    public EggCookingToHalfboiled(EggFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsInBowl && (_fsm.TimeSpentCooking >= 3f && _fsm.TimeSpentCooking < 6f);
    }
}