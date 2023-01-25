using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientContainerFSM : IStateMachine<IngredientContainerState, IngredientContainerFSM>, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField] private GameObject _errorAlert = null;
    [SerializeField] private Camera _relativeCamera = null;
    [SerializeField] private DragHandler _ingredient = null;

    // Null means that the FSM is not active/ can be considered complete
    private IState<IngredientContainerState, IngredientContainerFSM> _currentState = null;
    public override IState<IngredientContainerState, IngredientContainerFSM> CurrentState { get { return _currentState; } }

    // Chose to use dictionary as it is a logical structure for accessing identifiable states
    // Is neater than having individual variables for each class
    private Dictionary<IngredientContainerState, IState<IngredientContainerState, IngredientContainerFSM>> _allStates = new Dictionary<IngredientContainerState, IState<IngredientContainerState, IngredientContainerFSM>>();
    protected override Dictionary<IngredientContainerState, IState<IngredientContainerState, IngredientContainerFSM>> AllStates { get { return _allStates; } }
    
    public int Quantity = 10;
    public bool HasBeenRefilled = false;

    private WaitForSeconds _refillDelay = new WaitForSeconds(3f);

    void Start()
    {
        _ingredient.ExternalSetup(_relativeCamera);
        SetupStates();
    }

    void Update()
    {
        _currentState.Run();
    }

    // Click events originate from this object often, and usually needs to be
    // passed into the items they manage
    // PointerEventData is passed from this object to the next
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if(_currentState.GetName() == IngredientContainerState.Empty.ToString()) { return; }
        // Everytime one is released, reduce the quantity
        Quantity--;
        _ingredient.OnPointerUp(pointerEventData);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(_currentState.GetName() == IngredientContainerState.Empty.ToString())
        {
            StartCoroutine(Refill());
            return;
        }
        _ingredient.OnPointerDown(pointerEventData);
    }

    // Pass Drag events to DragHandler
    public void OnDrag(PointerEventData pointerEventData)
    {
        if(_currentState.GetName() == IngredientContainerState.Empty.ToString()) { return; }
        _ingredient.OnDrag(pointerEventData);
    }

    public override void Advance(IngredientContainerState nextState)
    {
        if(nextState == IngredientContainerState.End)
        {
            return;
        }
        if(_currentState != null) { _currentState.Exit(); }
        _currentState = AllStates[nextState];
        _currentState.Enter();
    }

    public override bool IsComplete()
    {
        return _currentState == null;
    }

    IEnumerator Refill()
    {
        // Delay the action
        yield return _refillDelay;
        // Refill and let the fsm know it is refilled
        Quantity = 10;
        HasBeenRefilled = true;
    }

    public void SetAlert(bool alertStatus)
    {
        _errorAlert.SetActive(alertStatus);
    }

    public void SetupStates()
    {
        AllStates.Add(IngredientContainerState.Filled, new IngredientContainerFilled(this));
        AllStates.Add(IngredientContainerState.Empty, new IngredientContainerEmpty(this));

        _currentState = AllStates[IngredientContainerState.Filled];
    }
}

public enum IngredientContainerState
{
    Filled,
    Empty,
    End
}
