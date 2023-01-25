using System.Collections.Generic;
using UnityEngine;

public class DrinkStirring : IState<DrinkState, DrinkFSM>
{
    private DrinkState _name = DrinkState.Stirring;
    private List<ITransition<DrinkState, DrinkFSM>> _transitions = new List<ITransition<DrinkState, DrinkFSM>>();
    public override List<ITransition<DrinkState, DrinkFSM>> Transitions { get { return _transitions; } }

    public DrinkStirring(DrinkFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void Run()
    {
        // Custom Run function for ticking time
        _fsm.TimeSpentStirring += Time.deltaTime;
        // Needs to be reincluded
        CheckTransitions();
    }

    public override void Enter()
    {
        _fsm.IsBeingStirred = true;
    }

    public override void Exit()
    {
        _fsm.IsBeingStirred = false;
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new DrinkStirringToEnd(_fsm));
        _transitions.Add(new DrinkStirringToReady(_fsm));
    }
}