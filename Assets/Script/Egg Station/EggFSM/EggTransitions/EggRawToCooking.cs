public class EggRawToCooking : ITransition<EggState, EggFSM>
{
    private EggState _targetState = EggState.Cooking;
    public override EggState TargetState { get { return _targetState; } }

    public EggRawToCooking(EggFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsBeingBoiled;
    }
}