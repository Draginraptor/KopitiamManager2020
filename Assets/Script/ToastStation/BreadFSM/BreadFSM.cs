using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BreadFSM : IStateMachine<BreadState, BreadFSM>, IPointerUpHandler, IPointerDownHandler
{
    public System.Action OnDropSucceeded = delegate { };
    public System.Action OnDropFailed = delegate { };
    public System.Action OnEnded = delegate { };
    public DragHandler DragHandler = null;

    [SerializeField] private GameObject _coverBread = null;
    [SerializeField] private GameObject _mainBread = null;
    [SerializeField] private GameObject _cutBread = null;
    [SerializeField] private MeshRenderer[] _cutFillings = null;
    [SerializeField] private Collider _collider = null;
    [SerializeField] private MeshRenderer[] _fillingLayers = null;
    [SerializeField] private BreadFillingMaterial[] _fillingMaterials = null;

    // Allow linking of DrinkState and the matching Drink model through the inspector
    [System.Serializable]
    public struct BreadFillingMaterial
    {
        public BreadIngredient BreadIngredientName;
        public Material FillingMaterial;
    }

    [SerializeField] private Material _untoastedCrust = null;
    [SerializeField] private Material _untoastedCenter = null;
    [SerializeField] private Material _toastCrust = null;
    [SerializeField] private Material _toastCenter = null;

    [SerializeField] private List<MeshRenderer> _breadCrusts = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> _breadCenters = new List<MeshRenderer>();

    [SerializeField] private List<BreadRecipe> _breadRecipes = new List<BreadRecipe>();

    private Vector3 _spawnPoint = new Vector3();
    // Null means that the FSM is not active/ can be considered complete
    private IState<BreadState, BreadFSM> _currentState = null;
    public override IState<BreadState, BreadFSM> CurrentState { get { return _currentState; } }

    // Chose to use dictionary as it is a logical structure for accessing identifiable states
    // Is neater than having individual variables for each class
    private Dictionary<BreadState, IState<BreadState, BreadFSM>> _allStates = new Dictionary<BreadState, IState<BreadState, BreadFSM>>();
    protected override Dictionary<BreadState, IState<BreadState, BreadFSM>> AllStates { get { return _allStates; } }
    private Dictionary<BreadIngredient, Material> _fillingMaterialsDict = new Dictionary<BreadIngredient, Material>();

    // Variables relevant to this FSM's transitions
    public bool IsInToaster = false;
    public bool HasBeenToasted = false;
    public bool IsOnPlate = false;
    public bool IsCovered = false;
    public bool IsReady = false;
    public bool IsPlacedOnTray = false;

    private IState<BreadState, BreadFSM> _defaultState;
    // Determines whether the FSM can run
    private bool _isPaused = true;
    // Variables to save the position of the object before it gets dragged for snapping it back
    private Vector3 _lastPosition = new Vector3();

    private List<BreadIngredient> _fillings;
    private int _maxFillings = 0;
    private int _currentFillingLevel = 0;

    public BreadType FinalBreadType = BreadType.Mismatch;
    
    void Awake()
    {
        _maxFillings = _fillingLayers.Length;
        _fillings = new List<BreadIngredient>();
        _spawnPoint = transform.position;
        SetupStates();
        SetupFillingMaterialsDict();
        
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

    private void SetupFillingMaterialsDict()
    {
        for(int i = 0; i < _fillingMaterials.Length; i++)
        {
            _fillingMaterialsDict.Add(_fillingMaterials[i].BreadIngredientName, _fillingMaterials[i].FillingMaterial);
        }
    }

    // If no drag is started (i.e. a click), the egg will still reset
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Pause(false);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        SavePosition();
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
        ResetToLastPosition();
        // If FSM is not currently running, nothing under the if statement needs to be run
        if(IsComplete()) { return; }
    }

    public void OnDropSucceed()
    {
        OnDropSucceeded();
    }

    private void SavePosition()
    {
        _lastPosition = transform.position;
    }

    private void ResetToLastPosition()
    {
        transform.position = _lastPosition;
    }

    public override void Advance(BreadState nextState)
    {
        if(nextState == BreadState.End)
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
        AllStates.Add(BreadState.Untoasted, new BreadUntoasted(this));
        AllStates.Add(BreadState.Toasting, new BreadToasting(this));
        AllStates.Add(BreadState.Toasted, new BreadToasted(this));
        AllStates.Add(BreadState.Filling, new BreadFilling(this));
        AllStates.Add(BreadState.Cutting, new BreadCutting(this));
        AllStates.Add(BreadState.Ready, new BreadReady(this));

        _defaultState = AllStates[BreadState.Untoasted];
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
        // Show the main bread, hide the cover and uncrusted
        _mainBread.SetActive(true);
        _coverBread.SetActive(false);
        _cutBread.SetActive(false);
        // Reset the fillings
        ClearFillings();
        // Set back to Untoasted
        ChangeToUntoastedMaterial();
        //Reset to spawn and allow drag
        transform.position = _spawnPoint;
        EnableDrag(true);
        // Reset variables
        IsInToaster = false;
        HasBeenToasted = false;
        IsOnPlate = false;
        IsCovered = false;
        IsReady = false;
        IsPlacedOnTray = false;
        FinalBreadType = BreadType.Mismatch;
    }

    public void ChangeToToastMaterial()
    {
        // Change the crusts to be toast crusts
        for(int i = 0; i < _breadCrusts.Count; i++)
        {
            _breadCrusts[i].material = _toastCrust;
        }
        // Change the centers to be toast centers
        for(int i = 0; i < _breadCenters.Count; i++)
        {
            _breadCenters[i].material = _toastCenter;
        }
    }

    public void ChangeToUntoastedMaterial()
    {
        // Change the crusts to be toast crusts
        for(int i = 0; i < _breadCrusts.Count; i++)
        {
            _breadCrusts[i].material = _untoastedCrust;
        }
        // Change the centers to be toast centers
        for(int i = 0; i < _breadCenters.Count; i++)
        {
            _breadCenters[i].material = _untoastedCenter;
        }
    }

    public void ClearFillings()
    {
        _fillings.Clear();
        _currentFillingLevel = 0;
        for(int i = 0; i < _fillingLayers.Length; i++)
        {
            _fillingLayers[i].gameObject.SetActive(false);
        }
    }

    public void AddFilling(BreadIngredient ingredient)
    {
        if(_currentFillingLevel >= _maxFillings) { return; }
        _fillings.Add(ingredient);
        _fillingLayers[_currentFillingLevel].material = _fillingMaterialsDict[ingredient];
        _fillingLayers[_currentFillingLevel].gameObject.SetActive(true);
        _currentFillingLevel++;
    }

    public void PrepareForCutting()
    {
        _coverBread.SetActive(true);
    }

    public void CutBread()
    {
        CheckBreadType();
        _mainBread.SetActive(false);
        _coverBread.SetActive(false);
        _cutBread.SetActive(true);
        for(int i = 0; i < _cutFillings.Length; i++)
        {
            int randomIndex = Random.Range(0, _fillings.Count);
            _cutFillings[i].material = _fillingMaterialsDict[_fillings[randomIndex]];
        }
    }

    public void CheckBreadType()
    {
        for(int i = 0; i < _breadRecipes.Count; i++)
        {
            if(_breadRecipes[i].QualifiesForRecipe(this, _fillings))
            {
                FinalBreadType = _breadRecipes[i].breadName;
            }
        }
    }
}

public enum BreadState
{
    Untoasted,
    Toasting,
    Toasted,
    Filling,
    Cutting,
    Ready,
    End
}

public enum BreadIngredient
{
    Kaya,
    Jam,
    Butter
}

public enum BreadType
{
    Kaya,
    Butter,
    KayaButter,
    Jam,
    KayaToast,
    ButterToast,
    KayaButterToast,
    JamToast,
    Mismatch
}