public class DrinkStirringToEnd : ITransition<DrinkState, DrinkFSM>
{
    private DrinkState _targetState = DrinkState.End;
    public override DrinkState TargetState { get { return _targetState; } }

    public DrinkStirringToEnd(DrinkFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsBeingClicked && _fsm.TimeSpentStirring >= 5f;
    }
}