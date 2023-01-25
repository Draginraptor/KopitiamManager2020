public class DispenserDispensingToPaused : ITransition<DispenserState, DispenserFSM>
{
    private DispenserState _targetState = DispenserState.Paused;
    public override DispenserState TargetState { get { return _targetState; } }

    public DispenserDispensingToPaused(DispenserFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.Quantity <= 0;
    }
}