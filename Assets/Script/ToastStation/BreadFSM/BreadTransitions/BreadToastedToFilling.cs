public class BreadToastedToFilling : ITransition<BreadState, BreadFSM>
{
    private BreadState _targetState = BreadState.Filling;
    public override BreadState TargetState { get { return _targetState; } }

    public BreadToastedToFilling(BreadFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsOnPlate;
    }
}