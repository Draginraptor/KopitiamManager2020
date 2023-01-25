using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DispenserFSM : IStateMachine<DispenserState, DispenserFSM>, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    // Gameobjects for alerting the player
    [SerializeField] private GameObject _pauseAlert = null;
    [SerializeField] private GameObject _errorAlert = null;

    // Component for handling drops
    [SerializeField] private DropHandler _dropHandler = null;

    // Position to place the dropped cup
    [SerializeField] private Transform _cupTransform = null;

    // What the dispenser will add to a cup
    [SerializeField] private DrinkIngredient _ingredientToDispense = DrinkIngredient.Water;
    [SerializeField] private float _dispenseDelayInSeconds = 1f;

    [HideInInspector] public DrinkFSM Drink = null;

    // Null means that the FSM is not active/ can be considered complete
    private IState<DispenserState, DispenserFSM> _currentState = null;
    public override IState<DispenserState, DispenserFSM> CurrentState { get { return _currentState; } }

    // Chose to use dictionary as it is a logical structure for accessing identifiable states
    // Is neater than having individual variables for each class
    private Dictionary<DispenserState, IState<DispenserState, DispenserFSM>> _allStates = new Dictionary<DispenserState, IState<DispenserState, DispenserFSM>>();
    protected override Dictionary<DispenserState, IState<DispenserState, DispenserFSM>> AllStates { get { return _allStates; } }
    
    // Amount in the dispenser
    public int Quantity = 10;
    public bool HasRefilled = false;
    private WaitForSeconds _dispenseDelay;
    private WaitForSeconds _refillDelay = new WaitForSeconds(3f);

    // Start is called before the first frame update
    void Start()
    {
        SetupStates();
        // Subscribe to DropHandler events to run code accordingly
        _dropHandler.OnDropReceived += CheckReceivedDrop;

        _dispenseDelay = new WaitForSeconds(_dispenseDelayInSeconds);
    }

    void Update()
    {
        _currentState.Run();
    }

    // Pass mouse events to the cup
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if(Drink == null) { return; }
        Drink.DragHandler.OnPointerUp(pointerEventData);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(_currentState.GetName() == DispenserState.Empty.ToString())
        {
            StartCoroutine(Refill());
            return;
        }
        if(Drink == null) { return; }
        Drink.DragHandler.OnPointerDown(pointerEventData);
        // As if removed from watery
        StopAllCoroutines();
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if(Drink == null) { return; }
        Drink.DragHandler.OnDrag(pointerEventData);
    }

    // Be alerted to what is happening to the Drink so that relevant functions can be run
    void OnDropSucceed()
    {
        OnDrinkRemoved();
    }

    void OnDropFail()
    {
        StartCoroutine(Dispense());
    }

    // Coroutine to dispense ingredients into cups over time
    IEnumerator Dispense()
    {
        while(HasDrink())
        {
            yield return _dispenseDelay;
            if(_currentState.GetName() == DispenserState.Paused.ToString()) { yield break;}
            Drink.AddingIngredient(_ingredientToDispense);
            Quantity--;
        }
    }

    // Checks if the drop received is relevant to the object by searching for the relevant components
    void CheckReceivedDrop(GameObject drop)
    {
        if(_currentState.GetName() == DispenserState.Filled.ToString())
        {
            // If theres no drink check for it
            if(Drink == null)
            {
                // Look for an DrinkFSM component
                DrinkFSM drink = drop.GetComponent<DrinkFSM>();
                if(drink != null)
                {
                    if(drink.CurrentState.GetName() == DrinkState.AddingIngredients.ToString())
                    {
                        Drink = drink;
                        _dropHandler.SucceedDrop();
                        OnDrinkAdded();
                        return;
                    }
                }
            }
        }
        
        // If nothing worked
        _dropHandler.FailDrop();
    }

    void OnDrinkAdded()
    {
        // Position and update Drink
        Drink.transform.SetParent(transform);
        Drink.transform.position = _cupTransform.position;
        Drink.IsOnDispenser = true;

        // Subscribe to interested events
        Drink.OnEnded += OnDrinkRemoved;
        Drink.OnDropSucceeded += OnDropSucceed;
        Drink.OnDropFailed += OnDropFail;
        
        // Begin dispensing
        StartCoroutine(Dispense());
    }   
    void OnDrinkRemoved()
    {
        // Update drink
        StopAllCoroutines();
        Drink.IsOnDispenser = false;

        // Unsubscribe - not needed anymore
        Drink.OnEnded -= OnDrinkRemoved;
        Drink.OnDropSucceeded -= OnDropSucceed;
        Drink.OnDropFailed -= OnDropFail;

        // Reset to base status
        Reset();
    }

    // Toggle the drop handler
    void EnableDropHandler(bool enable)
    {
        _dropHandler.enabled = enable;
    }

    void Reset()
    {
        Drink = null;
        EnableDropHandler(true);
    }
    
    public override void Advance(DispenserState nextState)
    {
        if(nextState == DispenserState.End)
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

    void SetupStates()
    {
        AllStates.Add(DispenserState.Filled, new DispenserFilled(this));
        AllStates.Add(DispenserState.Dispensing, new DispenserDispensing(this));
        AllStates.Add(DispenserState.Paused, new DispenserPaused(this));
        AllStates.Add(DispenserState.Empty, new DispenserEmpty(this));

        _currentState = AllStates[DispenserState.Filled];
    }

    public bool HasDrink()
    {
        return Drink != null;
    }

    IEnumerator Refill()
    {
        yield return _refillDelay;
        Quantity = 10;
        HasRefilled = true;
    }

    public void SetPause(bool pauseStatus)
    {
        _pauseAlert.SetActive(pauseStatus);
    }

    public void SetAlert(bool alertStatus)
    {
        _errorAlert.SetActive(alertStatus);
    }
}

public enum DispenserState
{
    Filled,
    Empty,
    Dispensing,
    Paused,
    End
}