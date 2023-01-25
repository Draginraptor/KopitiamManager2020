using System.Collections.Generic;

public class BreadReady : IState<BreadState, BreadFSM>
{
    private BreadState _name = BreadState.Ready;
    private List<ITransition<BreadState, BreadFSM>> _transitions = new List<ITransition<BreadState, BreadFSM>>();
    public override List<ITransition<BreadState, BreadFSM>> Transitions { get { return _transitions; } }

    public BreadReady(BreadFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }
    
    public override void Enter()
    {
        _fsm.CutBread();
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new BreadReadyToEnd(_fsm));
    }
}