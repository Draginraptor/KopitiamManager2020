public class IngredientContainerFilledToEmpty : ITransition<IngredientContainerState, IngredientContainerFSM>
{
    private IngredientContainerState _targetState = IngredientContainerState.Empty;
    public override IngredientContainerState TargetState { get { return _targetState; } }

    public IngredientContainerFilledToEmpty(IngredientContainerFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.Quantity <= 0;
    }
}