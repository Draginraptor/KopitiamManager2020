public abstract class ITransition<T, U> where U : IStateMachine<T, U>
{
    protected U _fsm;
    // State to transition to on transition success
    public abstract T TargetState { get; }
    // Check to run while relevant state is active
    public abstract bool TransitionCheck();
}