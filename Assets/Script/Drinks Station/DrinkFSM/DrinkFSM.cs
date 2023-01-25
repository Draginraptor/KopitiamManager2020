using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrinkFSM : IStateMachine<DrinkState, DrinkFSM>, IPointerUpHandler, IPointerDownHandler
{
    public System.Action OnDropSucceeded = delegate { };
    public System.Action OnDropFailed = delegate { };
    public System.Action OnEnded = delegate { };
    public DragHandler DragHandler = null;
    public DropHandler DropHandler = null;

    [SerializeField] private MeshRenderer _finalDrinkModel = null;
    [SerializeField] private List<DrinkTypeMaterial> _drinkMaterials = new List<DrinkTypeMaterial>();
    [SerializeField] private List<DrinkRecipe> _drinkRecipes = new List<DrinkRecipe>();
    [SerializeField] private List<DrinkIngredientMaterial> _drinkIngredientMaterials = new List<DrinkIngredientMaterial>();
    [SerializeField] private List<MeshRenderer> _drinkSections = new List<MeshRenderer>();
    [SerializeField] private GameObject _spoon = null;

    // Allow linking of DrinkState and the matching Drink model through the inspector
    [System.Serializable]
    public struct DrinkTypeMaterial
    {
        public DrinkType DrinkTypeName;
        public Material DrinkMaterial;
    }

    [System.Serializable]
    public struct DrinkIngredientMaterial
    {
        public DrinkIngredient DrinkIngredientName;
        public Material IngredientMaterial;
    }

    private Vector3 _spawnPoint = new Vector3();
    // Null means that the FSM is not active/ can be considered complete
    private IState<DrinkState, DrinkFSM> _currentState = null;
    public override IState<DrinkState, DrinkFSM> CurrentState { get { return _currentState; } }

    // Chose to use dictionary as it is a logical structure for accessing identifiable states
    // Is neater than having individual variables for each class
    private Dictionary<DrinkState, IState<DrinkState, DrinkFSM>> _allStates = new Dictionary<DrinkState, IState<DrinkState, DrinkFSM>>();
    protected override Dictionary<DrinkState, IState<DrinkState, DrinkFSM>> AllStates { get { return _allStates; } }

    // Variables relevant to this FSM's transitions
    public bool IsBeingClicked = false;
    public bool IsOnCoaster = false;
    public bool IsOnDispenser = false;
    public bool IsBeingStirred = false;
    public bool IsReady = false;
    public float TimeSpentStirring = 0f;
    private List<DrinkIngredient> _drinkIngredients = new List<DrinkIngredient>();
    public DrinkType FinalDrinkType = DrinkType.Muck;
    public bool IsPlacedOnTray = false;

    private IState<DrinkState, DrinkFSM> _defaultState;
    // Determines whether the FSM can run
    private bool _isPaused = true;
    // Variables to save the position of the object before it gets dragged for snapping it back
    private Vector3 _lastPosition = new Vector3();
    private IState<DrinkState, DrinkFSM> _lastState;
    // Dictionary for storing the allocated Drink models for easy retrieval
    private Dictionary<DrinkType, Material> _drinkMaterialsDict = new Dictionary<DrinkType, Material>();
    private Dictionary<DrinkIngredient, Material> _drinkIngredientMaterialsDict = new Dictionary<DrinkIngredient, Material>();
    
    void Awake()
    {
        _spawnPoint = transform.position;
        SetupStates();
        SetupDrinkMaterialsDict();
        SetupDrinkIngredientMaterialsDict();
        ClearIngredients();
        
        // When a drag is started without using OnBeginDrag
        DragHandler.OnClickReleased += OnPointerUp;
        DragHandler.OnDragStarted += OnPointerDown;
        DragHandler.OnDropFailed += OnDropFail;
        DragHandler.OnDropSucceeded += OnDropSucceed;

        DropHandler.OnDropReceived += CheckReceivedDrop;
    }

    void Update()
    {
        // If FSM should be paused don't do anything
        // Or if FSM is not running at all
        if(_isPaused || IsComplete()) { return; }
        _currentState.Run();
    }

    // If no drag is started (i.e. a click), the Drink will still reset
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        IsBeingClicked = false;
        Pause(false);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        IsBeingClicked = true;
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
        // If FSM is not currently running, nothing under the if statement needs to be run
        if(IsComplete()) { return; }
        OnDropFailed();
        Pause(false);
        ResetToLastPosition();
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

    public override void Advance(DrinkState nextState)
    {
        if(nextState == DrinkState.End)
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
        AllStates.Add(DrinkState.Empty, new DrinkEmpty(this));
        AllStates.Add(DrinkState.AddingIngredients, new DrinkAddingIngredients(this));
        AllStates.Add(DrinkState.Filling, new DrinkFilling(this));
        AllStates.Add(DrinkState.Stirring, new DrinkStirring(this));
        AllStates.Add(DrinkState.Ready, new DrinkReady(this));

        _defaultState = AllStates[DrinkState.Empty];
    }

    private void SetupDrinkMaterialsDict()
    {
        for(int i = 0; i < _drinkMaterials.Count; i++)
        {
            _drinkMaterialsDict.Add(_drinkMaterials[i].DrinkTypeName, _drinkMaterials[i].DrinkMaterial);
        }
    }

    private void SetupDrinkIngredientMaterialsDict()
    {
        for(int i = 0; i < _drinkIngredientMaterials.Count; i++)
        {
            _drinkIngredientMaterialsDict.Add(_drinkIngredientMaterials[i].DrinkIngredientName, _drinkIngredientMaterials[i].IngredientMaterial);
        }
    }

    private void Begin()
    {
        if(IsComplete())
        {
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

    // To prevent certain behaviours from running
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
    }

    private void Reset()
    {
        // Reset appearance and functionality
        ClearIngredients();
        transform.position = _spawnPoint;
        _finalDrinkModel.gameObject.SetActive(false);
        _spoon.SetActive(false);
        EnableDrag(true);

        // Reset FSM values
        IsOnCoaster = false;
        IsOnDispenser = false;
        IsBeingStirred = false;
        IsReady = false;
        TimeSpentStirring = 0f;
        IsPlacedOnTray = false;
        FinalDrinkType = DrinkType.Muck;
    }

    // public accessor
    public int IngredientsCount()
    {
        return _drinkIngredients.Count;
    }

    public void AddingIngredient(DrinkIngredient ingredient)
    {
        _drinkIngredients.Add(ingredient);
        UpdateIngredientDisplay();
    }

    // To show what the player has added
    private void UpdateIngredientDisplay()
    {
        // If the drink is overflowing, no need to update the display
        if(IngredientsCount() > 10) { return; }
        for(int i = 0; i < _drinkIngredients.Count; i++)
        {
            _drinkSections[i].material = _drinkIngredientMaterialsDict[_drinkIngredients[i]];
            _drinkSections[i].gameObject.SetActive(true);
        }
    }

    // Clean slate on the Drink
    private void ClearIngredients()
    {
        _drinkIngredients.Clear();
        // Hide all the sections
        for(int i = 0; i < _drinkSections.Count; i++)
        {
            _drinkSections[i].gameObject.SetActive(false);
        }
    }

    void CheckReceivedDrop(GameObject drop)
    {
        if(_currentState == null) { return; }
        // Only case where drink needs to receive drop is the AddingIngredients state
        if(_currentState.GetName() == DrinkState.AddingIngredients.ToString())
        {
            if(CheckForDrinkIngredientDrop(drop))
            {
                return;
            }
            else if(CheckForSpoonDrop(drop))
            {
                return;
            }
        }
        DropHandler.FailDrop();
    }

    bool CheckForDrinkIngredientDrop(GameObject drop)
    {
        // Look for the ingreditent object
        DrinkIngredientObj ingredient = drop.GetComponent<DrinkIngredientObj>();
        if(ingredient != null)
        {
            AddingIngredient(ingredient.DrinkIngredient);
            return true;
        }
        return false;
    }

    bool CheckForSpoonDrop(GameObject drop)
    {
        // Look for a spoon component (progresses to next state)
        Spoon spoon = drop.GetComponent<Spoon>();
        if(spoon != null)
        {
            _spoon.SetActive(true);
            IsBeingStirred = true;
            EnableDrag(false);
            StartCoroutine(WaitForStirringEnd());
            return true;
        }
        return false;
    }

    IEnumerator WaitForStirringEnd()
    {
        while(!IsReady)
        {
            yield return null;
        }
        _spoon.SetActive(false);
        IsBeingStirred = false;
        EnableDrag(true);
    }

    public void CheckDrinkType()
    {
        for(int i = 0; i < _drinkRecipes.Count; i++)
        {
            if(_drinkRecipes[i].QualifiesForRecipe(_drinkIngredients))
            {
                FinalDrinkType = _drinkRecipes[i].drinkName;
            }
        }

        DisplayDrinkType();
    }

    private void DisplayDrinkType()
    {
        ClearIngredients();
        _finalDrinkModel.gameObject.SetActive(true);
        _finalDrinkModel.material = _drinkMaterialsDict[FinalDrinkType];
    }
}

public enum DrinkState
{
    Empty,
    AddingIngredients,
    Filling,
    Stirring,
    Ready,
    End
}

public enum DrinkIngredient
{
    Water,
    MiloPowder,
    CoffeePowder,
    Tea,
    CondensedMilk
}

public enum DrinkType
{
    Water,
    Milo,
    Coffee,
    CoffeeWithMilk,
    Tea,
    TeaWithMilk,
    Muck
}