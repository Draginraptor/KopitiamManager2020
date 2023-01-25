using System.Collections.Generic;

public class DispenserEmpty : IState<DispenserState, DispenserFSM>
{
    private DispenserState _name = DispenserState.Empty;
    private List<ITransition<DispenserState, DispenserFSM>> _transitions = new List<ITransition<DispenserState, DispenserFSM>>();
    public override List<ITransition<DispenserState, DispenserFSM>> Transitions { get { return _transitions; } }

    public DispenserEmpty(DispenserFSM fsm)
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
        _fsm.SetAlert(false);
        // Reset check
        _fsm.HasRefilled = false;
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new DispenserEmptyToFilled(_fsm));
    }
}