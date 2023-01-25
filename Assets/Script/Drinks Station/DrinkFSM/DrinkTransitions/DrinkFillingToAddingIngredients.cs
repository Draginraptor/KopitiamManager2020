public class DrinkFillingToAddingIngredients : ITransition<DrinkState, DrinkFSM>
{
    private DrinkState _targetState = DrinkState.AddingIngredients;
    public override DrinkState TargetState { get { return _targetState; } }

    public DrinkFillingToAddingIngredients(DrinkFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsOnCoaster;
    }
}