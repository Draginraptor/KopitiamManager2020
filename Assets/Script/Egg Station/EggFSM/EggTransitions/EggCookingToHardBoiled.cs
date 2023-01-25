public class EggCookingToHardboiled : ITransition<EggState, EggFSM>
{
    private EggState _targetState = EggState.HardBoiled;
    public override EggState TargetState { get { return _targetState; } }

    public EggCookingToHardboiled(EggFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsInBowl && (_fsm.TimeSpentCooking >= 9f && _fsm.TimeSpentCooking < 12f);
    }
}