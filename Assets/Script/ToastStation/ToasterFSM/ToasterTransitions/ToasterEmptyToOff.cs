public class ToasterEmptyToOff : ITransition<ToasterState, ToasterFSM>
{
    private ToasterState _targetState = ToasterState.Off;
    public override ToasterState TargetState { get { return _targetState; } }

    public ToasterEmptyToOff(ToasterFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasBread();
    }
}