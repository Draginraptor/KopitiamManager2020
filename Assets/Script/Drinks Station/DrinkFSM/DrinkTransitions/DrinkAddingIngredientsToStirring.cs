public class DrinkAddingIngredientsToStirring : ITransition<DrinkState, DrinkFSM>
{
    private DrinkState _targetState = DrinkState.Stirring;
    public override DrinkState TargetState { get { return _targetState; } }

    public DrinkAddingIngredientsToStirring(DrinkFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsBeingStirred;
    }
}