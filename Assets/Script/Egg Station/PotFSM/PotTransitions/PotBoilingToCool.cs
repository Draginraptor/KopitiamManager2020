public class PotBoilingToCool : ITransition<PotState, PotFSM>
{
    private PotState _targetState = PotState.Cool;
    public override PotState TargetState { get { return _targetState; } }

    public PotBoilingToCool(PotFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return !_fsm.HasEgg() && _fsm.NumberOfUses > 3;
    }
}