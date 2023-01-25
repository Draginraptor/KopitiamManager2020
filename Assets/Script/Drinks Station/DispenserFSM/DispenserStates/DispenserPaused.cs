using System.Collections.Generic;

public class DispenserPaused : IState<DispenserState, DispenserFSM>
{
    private DispenserState _name = DispenserState.Paused;
    private List<ITransition<DispenserState, DispenserFSM>> _transitions = new List<ITransition<DispenserState, DispenserFSM>>();
    public override List<ITransition<DispenserState, DispenserFSM>> Transitions { get { return _transitions; } }

    public DispenserPaused(DispenserFSM fsm)
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
        // Alert pause
        _fsm.SetPause(true);
    }

    public override void Exit()
    {
        // Remove pause
        _fsm.SetPause(false);
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new DispenserPausedToEmpty(_fsm));
    }
}