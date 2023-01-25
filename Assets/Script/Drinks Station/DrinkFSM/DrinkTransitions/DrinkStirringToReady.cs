public class DrinkStirringToReady : ITransition<DrinkState, DrinkFSM>
{
    private DrinkState _targetState = DrinkState.Ready;
    public override DrinkState TargetState { get { return _targetState; } }

    public DrinkStirringToReady(DrinkFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsBeingClicked && _fsm.TimeSpentStirring >= 3f && _fsm.TimeSpentStirring < 5f;
    }
}