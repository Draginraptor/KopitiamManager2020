public class EggReadyToEnd : ITransition<EggState, EggFSM>
{
    private EggState _targetState = EggState.End;
    public override EggState TargetState { get { return _targetState; } }

    public EggReadyToEnd(EggFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsPlacedOnTray;
    }
}