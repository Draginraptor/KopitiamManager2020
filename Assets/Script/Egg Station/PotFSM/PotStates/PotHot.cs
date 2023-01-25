using System.Collections.Generic;

public class PotHot : IState<PotState, PotFSM>
{
    private PotState _name = PotState.Hot;
    private List<ITransition<PotState, PotFSM>> _transitions = new List<ITransition<PotState, PotFSM>>();
    public override List<ITransition<PotState, PotFSM>> Transitions { get { return _transitions; } }

    public PotHot(PotFSM fsm)
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
        _transitions.Add(new PotHotToBoiling(_fsm));
    }
}