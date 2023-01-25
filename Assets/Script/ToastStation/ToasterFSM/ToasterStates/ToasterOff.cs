using System.Collections.Generic;

public class ToasterOff : IState<ToasterState, ToasterFSM>
{
    private ToasterState _name = ToasterState.Off;
    private List<ITransition<ToasterState, ToasterFSM>> _transitions = new List<ITransition<ToasterState, ToasterFSM>>();
    public override List<ITransition<ToasterState, ToasterFSM>> Transitions { get { return _transitions; } }

    public ToasterOff(ToasterFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override void Enter()
    {
        _fsm.DisplayUntoastedBread(true);
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new ToasterOffToToasting(_fsm));
    }
}