public class EggCookingToSoftboiled : ITransition<EggState, EggFSM>
{
    private EggState _targetState = EggState.SoftBoiled;
    public override EggState TargetState { get { return _targetState; } }

    public EggCookingToSoftboiled(EggFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsInBowl && (_fsm.TimeSpentCooking >= 6f && _fsm.TimeSpentCooking < 9f);
    }
}