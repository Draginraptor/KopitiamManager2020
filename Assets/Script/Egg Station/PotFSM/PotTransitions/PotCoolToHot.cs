public class PotCoolToHot : ITransition<PotState, PotFSM>
{
    private PotState _targetState = PotState.Hot;
    public override PotState TargetState { get { return _targetState; } }

    public PotCoolToHot(PotFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasBeenHeated;
    }
}