public class ToasterOffToToasting : ITransition<ToasterState, ToasterFSM>
{
    private ToasterState _targetState = ToasterState.Toasting;
    public override ToasterState TargetState { get { return _targetState; } }

    public ToasterOffToToasting(ToasterFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasTappedToasterLever;
    }
}