using System.Collections.Generic;

public class ToasterDone : IState<ToasterState, ToasterFSM>
{
    private ToasterState _name = ToasterState.Done;
    private List<ITransition<ToasterState, ToasterFSM>> _transitions = new List<ITransition<ToasterState, ToasterFSM>>();
    public override List<ITransition<ToasterState, ToasterFSM>> Transitions { get { return _transitions; } }

    public ToasterDone(ToasterFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override void Enter()
    {
        // Change appearance of bread and lever
        _fsm.DisplayUntoastedBread(false);
        _fsm.MoveLeverUp();
        // Increment the uses
        _fsm.NumberOfUses++;
        // Alter the bread
        _fsm.ToastBread();
    }

    public override void Exit()
    {
        // Hide the display bread
        _fsm.HideBread();
        // Reset the toasting timer
        _fsm.TimeSpentToasting = 0f;
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new ToasterDoneToEmpty(_fsm));
        _transitions.Add(new ToasterDoneToDirty(_fsm));
    }
}