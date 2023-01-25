public class BreadUntoastedToToasting : ITransition<BreadState, BreadFSM>
{
    private BreadState _targetState = BreadState.Toasting;
    public override BreadState TargetState { get { return _targetState; } }

    public BreadUntoastedToToasting(BreadFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsInToaster;
    }
}