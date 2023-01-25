using System.Collections.Generic;

public class BreadFilling : IState<BreadState, BreadFSM>
{
    private BreadState _name = BreadState.Filling;
    private List<ITransition<BreadState, BreadFSM>> _transitions = new List<ITransition<BreadState, BreadFSM>>();
    public override List<ITransition<BreadState, BreadFSM>> Transitions { get { return _transitions; } }

    public BreadFilling(BreadFSM fsm)
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
        _transitions.Add(new BreadFillingToCutting(_fsm));
    }
}