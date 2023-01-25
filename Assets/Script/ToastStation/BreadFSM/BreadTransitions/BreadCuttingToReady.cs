public class BreadCuttingToReady : ITransition<BreadState, BreadFSM>
{
    private BreadState _targetState = BreadState.Ready;
    public override BreadState TargetState { get { return _targetState; } }

    public BreadCuttingToReady(BreadFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsReady;
    }
}