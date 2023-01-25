public class DrinkAddingIngredientsToFilling : ITransition<DrinkState, DrinkFSM>
{
    private DrinkState _targetState = DrinkState.Filling;
    public override DrinkState TargetState { get { return _targetState; } }

    public DrinkAddingIngredientsToFilling(DrinkFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IsOnDispenser;
    }
}