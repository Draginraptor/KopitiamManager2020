public class ToasterDoneToEmpty : ITransition<ToasterState, ToasterFSM>
{
    private ToasterState _targetState = ToasterState.Empty;
    public override ToasterState TargetState { get { return _targetState; } }

    public ToasterDoneToEmpty(ToasterFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return !_fsm.HasBread() && _fsm.NumberOfUses <= 3;
    }
}