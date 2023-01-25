public class ToasterDirtyToEmpty : ITransition<ToasterState, ToasterFSM>
{
    private ToasterState _targetState = ToasterState.Empty;
    public override ToasterState TargetState { get { return _targetState; } }

    public ToasterDirtyToEmpty(ToasterFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.HasBeenCleaned;
    }
}