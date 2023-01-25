using System.Collections.Generic;

public class DispenserFilled : IState<DispenserState, DispenserFSM>
{
    private DispenserState _name = DispenserState.Filled;
    private List<ITransition<DispenserState, DispenserFSM>> _transitions = new List<ITransition<DispenserState, DispenserFSM>>();
    public override List<ITransition<DispenserState, DispenserFSM>> Transitions { get { return _transitions; } }

    public DispenserFilled(DispenserFSM fsm)
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
        _transitions.Add(new DispenserFilledToDispensing(_fsm));
    }
}