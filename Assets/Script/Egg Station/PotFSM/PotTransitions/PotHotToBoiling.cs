public class PotHotToBoiling : ITransition<PotState, PotFSM>
{
    private PotState _targetState = PotState.Boiling;
    public override PotState TargetState { get { return _targetState; } }

    public PotHotToBoiling(PotFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasEgg();
    }
}