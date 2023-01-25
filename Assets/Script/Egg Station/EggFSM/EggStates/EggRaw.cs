using System.Collections.Generic;

public class EggRaw : IState<EggState, EggFSM>
{
    private EggState _name = EggState.Raw;
    private List<ITransition<EggState, EggFSM>> _transitions = new List<ITransition<EggState, EggFSM>>();
    public override List<ITransition<EggState, EggFSM>> Transitions { get { return _transitions; } }

    public EggRaw(EggFSM fsm)
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
        _transitions.Add(new EggRawToCooking(_fsm));
    }
}