public class DrinkEmptyToAddingIngredients : ITransition<DrinkState, DrinkFSM>
{
    private DrinkState _targetState = DrinkState.AddingIngredients;
    public override DrinkState TargetState { get { return _targetState; } }

    public DrinkEmptyToAddingIngredients(DrinkFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsOnCoaster;
    }
}