using System.Collections.Generic;
using UnityEngine;

public class ToasterToasting : IState<ToasterState, ToasterFSM>
{
    private ToasterState _name = ToasterState.Toasting;
    private List<ITransition<ToasterState, ToasterFSM>> _transitions = new List<ITransition<ToasterState, ToasterFSM>>();
    public override List<ITransition<ToasterState, ToasterFSM>> Transitions { get { return _transitions; } }

    public ToasterToasting(ToasterFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override void Run()
    {
        _fsm.TimeSpentToasting += Time.deltaTime;
        CheckTransitions();
    }

    public override void Exit()
    {
        _fsm.HasTappedToasterLever = false;
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new ToasterToastingToDone(_fsm));
    }
}