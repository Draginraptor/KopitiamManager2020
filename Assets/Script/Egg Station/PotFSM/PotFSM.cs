using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotFSM : IStateMachine<PotState, PotFSM>, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField] private GameObject _errorAlert = null;
    [SerializeField] private Transform _eggTransform = null;
    [SerializeField] private DropHandler _dropHandler = null;
    
    // Null means that the FSM is not active/ can be considered complete
    private IState<PotState, PotFSM> _currentState = null;
    public override IState<PotState, PotFSM> CurrentState { get { return _currentState; } }

    // Chose to use dictionary as it is a logical structure for accessing identifiable states
    // Is neater than having individual variables for each class
    private Dictionary<PotState, IState<PotState, PotFSM>> _allStates = new Dictionary<PotState, IState<PotState, PotFSM>>();
    protected override Dictionary<PotState, IState<PotState, PotFSM>> AllStates { get { return _allStates; } }
    
    // FSM variables
    public int NumberOfUses = 0;
    public bool HasBeenHeated = false;
    
    private EggFSM _eggInPot = null;
    private WaitForSeconds _reheatDelay = new WaitForSeconds(3f);

    // Start is called before the first frame update
    void Start()
    {
        SetupStates();

        _dropHandler.OnDropReceived += CheckReceivedDrop;
    }

    void Update()
    {
        _currentState.Run();
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if(_eggInPot == null) { return; }
        _eggInPot.DragHandler.OnPointerUp(pointerEventData);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(_currentState.GetName() == PotState.Cool.ToString())
        {
            StartCoroutine(ReheatPot());
            return;
        }
        if(_eggInPot == null) { return; }
        _eggInPot.DragHandler.OnPointerDown(pointerEventData);
        // As if removed from watery
        _eggInPot.IsBeingBoiled = false;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if(_eggInPot == null) { return; }
        _eggInPot.DragHandler.OnDrag(pointerEventData);
    }

    public override void Advance(PotState nextState)
    {
        if(nextState == PotState.End)
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

    void OnEggAdded()
    {
        // Setup parent child relation
        _eggInPot.transform.SetParent(transform);
        // Position the egg appropriately
        _eggInPot.transform.position = _eggTransform.position;

        // Update relevant EggFSM data
        _eggInPot.IsBeingBoiled = true;
        _eggInPot.OnDropSucceeded += OnDropSucceed;
        _eggInPot.OnDropFailed += OnDropFail;
    }

    void OnEggRemoved()
    {
        // Egg is currently out of pot so
        _eggInPot.IsBeingBoiled = false;
        _eggInPot.OnDropSucceeded -= OnDropSucceed;
        _eggInPot.OnDropFailed -= OnDropFail;
        _eggInPot = null;
    }

    void OnDropSucceed()
    {
        OnEggRemoved();
    }

    void OnDropFail()
    {
        _eggInPot.IsBeingBoiled = true;
    }

    // Checks if the drop received is relevant to the object by searching for the relevant components
    void CheckReceivedDrop(GameObject drop)
    {
        if(_currentState.GetName() == PotState.Hot.ToString())
        {
            // Look for the EggFSM component
            EggFSM egg = drop.GetComponent<EggFSM>();
            // Check that the egg is not null and that the pot is not in use
            if(egg != null && _eggInPot == null)
            {
                // Check that it's in an acceptable state
                if(egg.CurrentState.GetName() == EggState.Raw.ToString())
                {
                    _eggInPot = egg;
                    _dropHandler.SucceedDrop();
                    OnEggAdded();
                    return;
                }
            }
        }
        
        _dropHandler.FailDrop();
    }

    private void SetupStates()
    {
        AllStates.Add(PotState.Hot, new PotHot(this));
        AllStates.Add(PotState.Boiling, new PotBoiling(this));
        AllStates.Add(PotState.Cool, new PotCool(this));

        _currentState = AllStates[PotState.Hot];
    }

    public bool HasEgg()
    {
        return _eggInPot != null;
    }

    public void SetAlert(bool alertStatus)
    {
        _errorAlert.SetActive(alertStatus);
    }

    IEnumerator ReheatPot()
    {
        yield return _reheatDelay;
        HasBeenHeated = true;
    }
}
public enum PotState
{
    Hot,
    Boiling,
    Cool,
    End
}