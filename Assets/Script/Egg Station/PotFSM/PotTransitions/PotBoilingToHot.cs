public class PotBoilingToHot : ITransition<PotState, PotFSM>
{
    private PotState _targetState = PotState.Hot;
    public override PotState TargetState { get { return _targetState; } }

    public PotBoilingToHot(PotFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return !_fsm.HasEgg() && _fsm.NumberOfUses <= 3;
    }
}