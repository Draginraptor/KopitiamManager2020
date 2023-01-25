public class EggCookingToRaw : ITransition<EggState, EggFSM>
{
    private EggState _targetState = EggState.Raw;
    public override EggState TargetState { get { return _targetState; } }

    public EggCookingToRaw(EggFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return !_fsm.IsBeingBoiled && _fsm.TimeSpentCooking < 3f;
    }
}