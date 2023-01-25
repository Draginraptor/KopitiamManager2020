public class ToasterToastingToDone : ITransition<ToasterState, ToasterFSM>
{
    private ToasterState _targetState = ToasterState.Done;
    public override ToasterState TargetState { get { return _targetState; } }

    public ToasterToastingToDone(ToasterFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.TimeSpentToasting > 10f;
    }
}