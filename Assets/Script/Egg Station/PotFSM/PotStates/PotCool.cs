using System.Collections.Generic;

public class PotCool : IState<PotState, PotFSM>
{
    private PotState _name = PotState.Cool;
    private List<ITransition<PotState, PotFSM>> _transitions = new List<ITransition<PotState, PotFSM>>();
    public override List<ITransition<PotState, PotFSM>> Transitions { get { return _transitions; } }

    public PotCool(PotFSM fsm)
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
        // Reset uses and the reheat check
        _fsm.NumberOfUses = 0;
        _fsm.HasBeenHeated = false;
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new PotCoolToHot(_fsm));
    }
}