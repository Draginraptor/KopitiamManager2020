public class DispenserFilledToDispensing : ITransition<DispenserState, DispenserFSM>
{
    private DispenserState _targetState = DispenserState.Dispensing;
    public override DispenserState TargetState { get { return _targetState; } }

    public DispenserFilledToDispensing(DispenserFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasDrink();
    }
}