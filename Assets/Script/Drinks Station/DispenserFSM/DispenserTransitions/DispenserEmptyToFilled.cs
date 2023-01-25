public class DispenserEmptyToFilled : ITransition<DispenserState, DispenserFSM>
{
    private DispenserState _targetState = DispenserState.Filled;
    public override DispenserState TargetState { get { return _targetState; } }

    public DispenserEmptyToFilled(DispenserFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasRefilled;
    }
}