public class IngredientContainerEmptyToFilled : ITransition<IngredientContainerState, IngredientContainerFSM>
{
    private IngredientContainerState _targetState = IngredientContainerState.Filled;
    public override IngredientContainerState TargetState { get { return _targetState; } }

    public IngredientContainerEmptyToFilled(IngredientContainerFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasBeenRefilled;
    }
}