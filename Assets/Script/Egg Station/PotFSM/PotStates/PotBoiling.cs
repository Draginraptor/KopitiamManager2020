using System.Collections.Generic;

public class PotBoiling : IState<PotState, PotFSM>
{
    private PotState _name = PotState.Boiling;
    private List<ITransition<PotState, PotFSM>> _transitions = new List<ITransition<PotState, PotFSM>>();
    public override List<ITransition<PotState, PotFSM>> Transitions { get { return _transitions; } }

    public PotBoiling(PotFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void Exit()
    {
        // Increment uses
        _fsm.NumberOfUses++;
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new PotBoilingToHot(_fsm));
        _transitions.Add(new PotBoilingToCool(_fsm));
    }
}