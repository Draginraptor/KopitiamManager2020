using System.Collections.Generic;

public class ToasterEmpty : IState<ToasterState, ToasterFSM>
{
    private ToasterState _name = ToasterState.Empty;
    private List<ITransition<ToasterState, ToasterFSM>> _transitions = new List<ITransition<ToasterState, ToasterFSM>>();
    public override List<ITransition<ToasterState, ToasterFSM>> Transitions { get { return _transitions; } }

    public ToasterEmpty(ToasterFSM fsm)
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
        _transitions.Add(new ToasterEmptyToOff(_fsm));
    }
}