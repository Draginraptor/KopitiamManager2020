public class BreadReadyToEnd : ITransition<BreadState, BreadFSM>
{
    private BreadState _targetState = BreadState.End;
    public override BreadState TargetState { get { return _targetState; } }

    public BreadReadyToEnd(BreadFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsPlacedOnTray;
    }
}