using System.Collections.Generic;

public class DrinkReady : IState<DrinkState, DrinkFSM>
{
    private DrinkState _name = DrinkState.Ready;
    private List<ITransition<DrinkState, DrinkFSM>> _transitions = new List<ITransition<DrinkState, DrinkFSM>>();
    public override List<ITransition<DrinkState, DrinkFSM>> Transitions { get { return _transitions; } }

    public DrinkReady(DrinkFSM fsm)
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
        // Find out what the player made
        _fsm.IsReady = true;
        _fsm.CheckDrinkType();
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new DrinkReadyToEnd(_fsm));
    }
}