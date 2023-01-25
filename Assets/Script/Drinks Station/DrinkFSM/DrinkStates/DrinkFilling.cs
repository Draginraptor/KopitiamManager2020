using System.Collections.Generic;

public class DrinkFilling : IState<DrinkState, DrinkFSM>
{
    private DrinkState _name = DrinkState.Filling;
    private List<ITransition<DrinkState, DrinkFSM>> _transitions = new List<ITransition<DrinkState, DrinkFSM>>();
    public override List<ITransition<DrinkState, DrinkFSM>> Transitions { get { return _transitions; } }

    public DrinkFilling(DrinkFSM fsm)
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
        _transitions.Add(new DrinkFillingToAddingIngredients(_fsm));
        _transitions.Add(new DrinkFillingToEnd(_fsm));
    }
}