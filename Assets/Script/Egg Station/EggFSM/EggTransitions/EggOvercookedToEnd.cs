public class EggOvercookedToEnd : ITransition<EggState, EggFSM>
{
    private EggState _targetState = EggState.End;
    public override EggState TargetState { get { return _targetState; } }

    public EggOvercookedToEnd(EggFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return true;
    }
}