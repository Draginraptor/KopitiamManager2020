using System.Collections.Generic;

public class IngredientContainerFilled : IState<IngredientContainerState, IngredientContainerFSM>
{
    private IngredientContainerState _name = IngredientContainerState.Filled;
    private List<ITransition<IngredientContainerState, IngredientContainerFSM>> _transitions = new List<ITransition<IngredientContainerState, IngredientContainerFSM>>();
    public override List<ITransition<IngredientContainerState, IngredientContainerFSM>> Transitions { get { return _transitions; } }

    public IngredientContainerFilled(IngredientContainerFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new IngredientContainerFilledToEmpty(_fsm));
    }
}