using System.Collections.Generic;
using UnityEngine;

public class EggCooking : IState<EggState, EggFSM>
{
    private EggState _name = EggState.Cooking;
    private List<ITransition<EggState, EggFSM>> _transitions = new List<ITransition<EggState, EggFSM>>();
    public override List<ITransition<EggState, EggFSM>> Transitions { get { return _transitions; } }

    public EggCooking(EggFSM fsm)
    {
        _fsm = fsm;
        SetupTransitions();
    }

    public override string GetName()
    {
        return _name.ToString();
    }

    // Custom run function to tick time
    public override void Run()
    {
        _fsm.TimeSpentCooking += Time.deltaTime;
        CheckTransitions();
    }

    public override void SetupTransitions()
    {
        _transitions.Add(new EggCookingToRaw(_fsm));
        _transitions.Add(new EggCookingToHalfboiled(_fsm));
        _transitions.Add(new EggCookingToSoftboiled(_fsm));
        _transitions.Add(new EggCookingToHardboiled(_fsm));
        _transitions.Add(new EggCookingToOvercooked(_fsm));
    }
}