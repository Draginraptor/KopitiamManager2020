using System.Collections.Generic;

public class DispenserDispensing : IState<DispenserState, DispenserFSM>
{
    private DispenserState _name = DispenserState.Dispensing;
    private List<ITransition<DispenserState, DispenserFSM>> _transitions = new List<ITransition<DispenserState, DispenserFSM>>();
    public override List<ITransition<DispenserState, DispenserFSM>> Transitions { get { return _transitions; } }

    public DispenserDispensing(DispenserFSM fsm)
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
        _transitions.Add(new DispenserDispensingToFilled(_fsm));
        _transitions.Add(new DispenserDispensingToPaused(_fsm));
    }
}