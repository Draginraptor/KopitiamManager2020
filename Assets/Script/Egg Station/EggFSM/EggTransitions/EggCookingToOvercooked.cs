public class EggCookingToOvercooked : ITransition<EggState, EggFSM>
{
    private EggState _targetState = EggState.Overcooked;
    public override EggState TargetState { get { return _targetState; } }

    public EggCookingToOvercooked(EggFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsInBowl && _fsm.TimeSpentCooking >= 12f;
    }
}