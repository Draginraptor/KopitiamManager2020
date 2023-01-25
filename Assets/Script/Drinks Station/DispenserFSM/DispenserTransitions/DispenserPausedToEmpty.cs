public class DispenserPausedToEmpty : ITransition<DispenserState, DispenserFSM>
{
    private DispenserState _targetState = DispenserState.Empty;
    public override DispenserState TargetState { get { return _targetState; } }

    public DispenserPausedToEmpty(DispenserFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return !_fsm.HasDrink();
    }
}