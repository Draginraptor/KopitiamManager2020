using System.Collections.Generic;

public class ToasterDirty : IState<ToasterState, ToasterFSM>
{
    private ToasterState _name = ToasterState.Dirty;
    private List<ITransition<ToasterState, ToasterFSM>> _transitions = new List<ITransition<ToasterState, ToasterFSM>>();
    public override List<ITransition<ToasterState, ToasterFSM>> Transitions { get { return _transitions; } }

    public ToasterDirty(ToasterFSM fsm)
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
        _fsm.SetAlert(true);
    }
    public override void Exit()
    {
        // Disable the notification
        _fsm.SetAlert(false);
        // Reset the variables
        _fsm.HasBeenCleaned = false;
        _fsm.NumberOfUses = 0;
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new ToasterDirtyToEmpty(_fsm));
    }
}