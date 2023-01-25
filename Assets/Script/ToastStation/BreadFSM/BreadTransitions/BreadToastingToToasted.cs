public class BreadToastingToToasted : ITransition<BreadState, BreadFSM>
{
    private BreadState _targetState = BreadState.Toasted;
    public override BreadState TargetState { get { return _targetState; } }

    public BreadToastingToToasted(BreadFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasBeenToasted;
    }
}