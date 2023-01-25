public class DrinkFillingToEnd : ITransition<DrinkState, DrinkFSM>
{
    private DrinkState _targetState = DrinkState.End;
    public override DrinkState TargetState { get { return _targetState; } }

    public DrinkFillingToEnd(DrinkFSM fsm)
    {
        _fsm = fsm;
    }

    public override bool TransitionCheck()
    {
        return _fsm.IngredientsCount() > 10;
    }
}