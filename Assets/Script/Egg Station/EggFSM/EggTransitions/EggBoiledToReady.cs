public class EggBoiledToReady : ITransition<EggState, EggFSM>
{
    private EggState _targetState = EggState.Ready;
    public override EggState TargetState { get { return _targetState; } }

    public EggBoiledToReady(EggFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasSeasoningSet;
    }
}