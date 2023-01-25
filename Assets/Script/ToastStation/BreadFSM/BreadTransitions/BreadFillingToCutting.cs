public class BreadFillingToCutting : ITransition<BreadState, BreadFSM>
{
    private BreadState _targetState = BreadState.Cutting;
    public override BreadState TargetState { get { return _targetState; } }

    public BreadFillingToCutting(BreadFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsCovered;
    }
}