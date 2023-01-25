using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EggFSM : IStateMachine<EggState, EggFSM>, IPointerUpHandler, IPointerDownHandler
{
    public System.Action OnDropSucceeded = delegate { };
    public System.Action OnDropFailed = delegate { };
    public System.Action OnEnded = delegate { };
    public DragHandler DragHandler = null;

    [SerializeField] private Collider _collider = null;
    [SerializeField] private GameObject _activeEggModel = null;
    [SerializeField] private List<EggModel> _eggModels = new List<EggModel>();

    // Allow linking of EggState and the matching Egg model through the inspector
    [System.Serializable]
    public struct EggModel
    {
        public EggState EggState;
        public GameObject EggObject;
    }

    private Vector3 _spawnPoint = new Vector3();
    // Null means that the FSM is not active/ can be considered complete
    private IState<EggState, EggFSM> _currentState = null;
    public override IState<EggState, EggFSM> CurrentState { get { return _currentState; } }

    // Chose to use dictionary as it is a logical structure for accessing identifiable states
    // Is neater than having individual variables for each class
    private Dictionary<EggState, IState<EggState, EggFSM>> _allStates = new Dictionary<EggState, IState<EggState, EggFSM>>();
    protected override Dictionary<EggState, IState<EggState, EggFSM>> AllStates { get { return _allStates; } }


    // Variables relevant to this FSM's transitions
    public bool IsBeingBoiled = false;
    public bool IsInBowl = false;
    public bool HasSeasoningSet = false;
    public bool IsReady = false;
    public float TimeSpentCooking = 0f;
    // Combination of private/public variables to enable custom get/set
    private EggState _eggBoiledLevel = EggState.Raw;
    public EggState EggBoiledLevel
    {
        get { return _eggBoiledLevel; }
        set
        {
            // Whenever the boiled level of the egg changes, change the model
            Debug.Log(value);
            _eggBoiledLevel = value;
            ChangeEggModel(_eggBoiledLevel);
        }
    }
    public bool IsPlacedOnTray = false;

    private IState<EggState, EggFSM> _defaultState;
    // Determines whether the FSM can run
    private bool _isPaused = true;
    // Variables to save the position of the object before it gets dragged for snapping it back
    private Vector3 _lastPosition = new Vector3();
    private IState<EggState, EggFSM> _lastState;
    // Dictionary for storing the allocated egg models for easy retrieval
    private Dictionary<EggState, GameObject> _eggModelsDict = new Dictionary<EggState, GameObject>();
    
    void Awake()
    {
        _spawnPoint = transform.position;
        SetupStates();
        SetupEggModelsDict();
        
        // When a drag is started without using OnBeginDrag
        DragHandler.OnClickReleased += OnPointerUp;
        DragHandler.OnDragStarted += OnPointerDown;
        DragHandler.OnDropFailed += OnDropFail;
        DragHandler.OnDropSucceeded += OnDropSucceed;
    }

    void Update()
    {
        // If FSM should be paused don't do anything
        // Or if FSM is not running at all
        if(_isPaused || IsComplete()) { return; }
        _currentState.Run();
    }

    // If no drag is started (i.e. a click), the egg will still reset
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Pause(false);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        SaveState();
        // If FSM is not currently running, nothing under the if statement needs to be run
        if(IsComplete())
        {
            Begin();
            return;
        }
        Pause(true);
    }

    public void OnDropFail()
    {
        OnDropFailed();
        Pause(false);
        ResetToLastState();
        // If FSM is not currently running, nothing under the if statement needs to be run
        if(IsComplete()) { return; }
    }

    public void OnDropSucceed()
    {
        OnDropSucceeded();
    }

    private void SaveState()
    {
        _lastPosition = transform.position;
        _lastState = _currentState;
    }

    private void ResetToLastState()
    {
        transform.position = _lastPosition;
        _currentState = _lastState;
    }

    public override void Advance(EggState nextState)
    {
        if(nextState == EggState.End)
        {
            End();
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

    private void SetupStates()
    {
        AllStates.Add(EggState.Raw, new EggRaw(this));
        AllStates.Add(EggState.Cooking, new EggCooking(this));
        AllStates.Add(EggState.HalfBoiled, new EggHalfBoiled(this));
        AllStates.Add(EggState.SoftBoiled, new EggSoftBoiled(this));
        AllStates.Add(EggState.HardBoiled, new EggHardBoiled(this));
        AllStates.Add(EggState.Overcooked, new EggOvercooked(this));
        AllStates.Add(EggState.Ready, new EggReady(this));

        _defaultState = AllStates[EggState.Raw];
    }

    private void SetupEggModelsDict()
    {
        for(int i = 0; i < _eggModels.Count; i++)
        {
            _eggModelsDict.Add(_eggModels[i].EggState, _eggModels[i].EggObject);
        }
    }

    public void ChangeEggModel(EggState eggState)
    {
        // Set the previous model to be inactive
        _activeEggModel.SetActive(false);
        // Reassign the active model and set it to be active
        _activeEggModel = _eggModelsDict[eggState];
        _activeEggModel.SetActive(true);
    }

    private void Begin()
    {
        if(IsComplete())
        {
            Debug.Log(_defaultState);
            _currentState = _defaultState;
            Pause(true);
        }
    }

    private void End()
    {
        OnEnded();
        _currentState = null;
        Reset();
        Pause(true);
    }

    private void Pause(bool pause)
    {
        if(_currentState != null)
        {
            _currentState.Run();
        }        
        _isPaused = pause;
    }

    public void EnableDrag(bool enable)
    {
        DragHandler.enabled = enable;
        _collider.enabled = enable;
    }

    private void Reset()
    {
        transform.position = _spawnPoint;
        EnableDrag(true);
        IsBeingBoiled = false;
        IsInBowl = false;
        HasSeasoningSet = false;
        IsReady = false;
        TimeSpentCooking = 0f;
        EggBoiledLevel = EggState.Raw;
        IsPlacedOnTray = false;
    }
}

public enum EggState
{
    Raw,
    Cooking,
    HalfBoiled,
    SoftBoiled,
    HardBoiled,
    Ready,
    Overcooked,
    End
}