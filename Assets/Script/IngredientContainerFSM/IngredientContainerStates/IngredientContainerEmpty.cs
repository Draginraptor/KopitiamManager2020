using System.Collections.Generic;

public class IngredientContainerEmpty : IState<IngredientContainerState, IngredientContainerFSM>
{
    private IngredientContainerState _name = IngredientContainerState.Empty;
    private List<ITransition<IngredientContainerState, IngredientContainerFSM>> _transitions = new List<ITransition<IngredientContainerState, IngredientContainerFSM>>();
    public override List<ITransition<IngredientContainerState, IngredientContainerFSM>> Transitions { get { return _transitions; } }

    public IngredientContainerEmpty(IngredientContainerFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void Enter()
    {
        // Alert error
        _fsm.SetAlert(true);
    }

    public override void Exit()
    {
        // Bring down alert
        _fsm.SetAlert(false);
        // Reset refill check
        _fsm.HasBeenRefilled = false;
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new IngredientContainerEmptyToFilled(_fsm));
    }
}