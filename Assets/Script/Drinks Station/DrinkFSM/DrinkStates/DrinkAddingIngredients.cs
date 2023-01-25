using System.Collections.Generic;

public class DrinkAddingIngredients : IState<DrinkState, DrinkFSM>
{
    private DrinkState _name = DrinkState.AddingIngredients;
    private List<ITransition<DrinkState, DrinkFSM>> _transitions = new List<ITransition<DrinkState, DrinkFSM>>();
    public override List<ITransition<DrinkState, DrinkFSM>> Transitions { get { return _transitions; } }

    public DrinkAddingIngredients(DrinkFSM fsm)
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
        _transitions.Add(new DrinkAddingIngredientsToEnd(_fsm));
        _transitions.Add(new DrinkAddingIngredientsToFilling(_fsm));
        _transitions.Add(new DrinkAddingIngredientsToStirring(_fsm));
    }
}