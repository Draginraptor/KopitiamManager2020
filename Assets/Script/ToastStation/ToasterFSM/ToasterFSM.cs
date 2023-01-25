using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToasterFSM : IStateMachine<ToasterState,ToasterFSM>, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField] private GameObject _errorAlert = null;
    [SerializeField] private DropHandler _dropHandler = null;
    [SerializeField] private ToasterLever _toasterLever = null;
    [SerializeField] private Transform _leverUpTransform = null;
    [SerializeField] private Transform _leverDownTransform = null;
    [SerializeField] private Transform _breadTransform = null;
    [SerializeField] private Transform _breadUpTransform = null;
    [SerializeField] private Transform _breadDownTransform = null;
    [SerializeField] private GameObject _untoastedDisplay = null;
    [SerializeField] private GameObject _toastedDisplay = null;
    [SerializeField] private GameObject _breadSlots = null;

    // Null means that the FSM is not active/ can be considered complete
    private IState<ToasterState, ToasterFSM> _currentState = null;
    public override IState<ToasterState, ToasterFSM> CurrentState { get { return _currentState; } }

    // Chose to use dictionary as it is a logical structure for accessing identifiable states
    // Is neater than having individual variables for each class
    private Dictionary<ToasterState, IState<ToasterState, ToasterFSM>> _allStates = new Dictionary<ToasterState, IState<ToasterState, ToasterFSM>>();
    protected override Dictionary<ToasterState, IState<ToasterState, ToasterFSM>> AllStates { get { return _allStates; } }

    // FSM related variables
    public bool HasTappedToasterLever = false;
    public int NumberOfUses = 0;
    public float TimeSpentToasting = 0f;
    public bool HasBeenCleaned = false;

    private BreadFSM _breadInToaster = null;

    private Vector3 _angleToRotateBread = new Vector3(90f, 0f, 0f);
    private WaitForSeconds _cleaningDelay = new WaitForSeconds(3f);

    // Start is called before the first frame update
    void Start()
    {
        SetupStates();

        _toasterLever.OnLeverTapped += HandleLeverTap;
        _dropHandler.OnDropReceived += CheckReceivedDrop;
    }

    void Update()
    {
        _currentState.Run();
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if(_breadInToaster == null || _currentState.GetName() != ToasterState.Done.ToString()) { return; }
        _breadInToaster.DragHandler.OnPointerUp(pointerEventData);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(_currentState.GetName() == ToasterState.Dirty.ToString())
        {
            StartCoroutine(CleanToaster());
            return;
        }
        if(_breadInToaster == null || _currentState.GetName() != ToasterState.Done.ToString()) { return; }
        _breadInToaster.DragHandler.OnPointerDown(pointerEventData);
    }

    IEnumerator CleanToaster()
    {
        yield return _cleaningDelay;
        HasBeenCleaned = true;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        // Will not run if there is no bread or if the toaster is not done
        if(_breadInToaster == null || _currentState.GetName() != ToasterState.Done.ToString()) { return; }
        _breadInToaster.DragHandler.OnDrag(pointerEventData);
    }

    public override void Advance(ToasterState nextState)
    {
        if(nextState == ToasterState.End)
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
        AllStates.Add(ToasterState.Empty, new ToasterEmpty(this));
        AllStates.Add(ToasterState.Off, new ToasterOff(this));
        AllStates.Add(ToasterState.Toasting, new ToasterToasting(this));
        AllStates.Add(ToasterState.Done, new ToasterDone(this));
        AllStates.Add(ToasterState.Dirty, new ToasterDirty(this));

        _currentState = AllStates[ToasterState.Empty];
    }

    public bool HasBread()
    {
        return _breadInToaster != null;
    }

    void OnBreadAdded()
    {
        // Setup parent child relation
        _breadInToaster.transform.SetParent(_breadTransform);
        // Position the bread appropriately
        _breadInToaster.transform.position = _breadTransform.position;
        _breadInToaster.transform.eulerAngles += _angleToRotateBread;
        _breadInToaster.EnableDrag(false);

        // Update relevant BreadFSM data
        _breadInToaster.IsInToaster = true;
        _breadInToaster.OnDropSucceeded += OnDropSucceed;
        _breadInToaster.OnDropFailed += OnDropFail;
    }

    void OnBreadRemoved()
    {
        _breadInToaster.transform.eulerAngles -= _angleToRotateBread;

        _breadInToaster.IsInToaster = false;
        _breadInToaster.OnDropSucceeded -= OnDropSucceed;
        _breadInToaster.OnDropFailed -= OnDropFail;
        _breadInToaster = null;
    }

    void OnDropSucceed()
    {
        OnBreadRemoved();
    }

    void OnDropFail()
    {
        _breadInToaster.IsInToaster = true;
    }

    // Checks if the drop received is relevant to the object by searching for the relevant components
    void CheckReceivedDrop(GameObject drop)
    {
        if(_currentState.GetName() == ToasterState.Empty.ToString())
        {
            // Look for the BreadFSM component
            BreadFSM bread = drop.GetComponent<BreadFSM>();
            // Check that the bread is not null and that the toaster is not in use
            if(bread != null && _breadInToaster == null)
            {
                // Check that it's in an acceptable state
                if(bread.CurrentState.GetName() == BreadState.Untoasted.ToString())
                {
                    _breadInToaster = bread;
                    _dropHandler.SucceedDrop();
                    OnBreadAdded();
                    return;
                }
            }
        }
        
        _dropHandler.FailDrop();
    }

    void HandleLeverTap()
    {
        if(_currentState.GetName() != ToasterState.Off.ToString()) { return; }
        HasTappedToasterLever = true;
        MoveLeverDown();
    }

    public void MoveLeverUp()
    {
        _toasterLever.transform.position = _leverUpTransform.position;
        _breadSlots.transform.position = _breadUpTransform.position;
    }

    public void MoveLeverDown()
    {
        _toasterLever.transform.position = _leverDownTransform.position;
        _breadSlots.transform.position = _breadDownTransform.position;
    }

    public void DisplayUntoastedBread(bool isUntoasted)
    {
        _untoastedDisplay.SetActive(isUntoasted);
        _toastedDisplay.SetActive(!isUntoasted);
    }

    public void HideBread()
    {
        _untoastedDisplay.SetActive(false);
        _toastedDisplay.SetActive(false);
    }

    public void ToastBread()
    {
        _breadInToaster.HasBeenToasted = true;
    }

    private void End()
    {
        _currentState = null;
    }

    public void SetAlert(bool alertStatus)
    {
        _errorAlert.SetActive(alertStatus);
    }
}

public enum ToasterState
{
    Empty,
    Off,
    Toasting,
    Done,
    Dirty,
    End
}