using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bowl : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private DragHandler _dragHandler = null;
    [SerializeField] private DropHandler _dropHandler = null;
    [SerializeField] private Transform _eggTransform = null;
    [SerializeField] private GameObject _seasoningSet = null;
    [HideInInspector] public EggFSM Egg = null;

    private Vector3 _lastPosition = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to DragHandler and DropHandler events to run code accordingly
        _dragHandler.OnClickReleased += OnPointerUp;
        _dragHandler.OnDragStarted += OnPointerDown;
        _dropHandler.OnDropReceived += CheckReceivedDrop;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        ResetToLastPosition();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        SavePosition();
    }

    // Checks if the drop received is relevant to the object by searching for the relevant components
    void CheckReceivedDrop(GameObject drop)
    {
        // If theres no egg check for it
        if(Egg == null)
        {
            // Look for an EggFSM component
            EggFSM egg = drop.GetComponent<EggFSM>();
            if(egg != null)
            {
                if(egg.CurrentState.GetName() != EggState.Raw.ToString())
                {
                    Egg = egg;
                    OnEggAdded();
                    _dropHandler.SucceedDrop();
                    return;
                }
            }
        }
        else if(Egg.CurrentState.GetName() != EggState.Overcooked.ToString())
        {
            // Look for SeasoningSet component
            SeasoningSet seasoningSet = drop.GetComponent<SeasoningSet>();
            if(seasoningSet != null)
            {
                AddSeasoningSet();
                return;
            }
        }
        _dropHandler.FailDrop();
    }

    void OnEggAdded()
    {
        Egg.transform.SetParent(transform);
        Egg.transform.position = _eggTransform.position;
        Egg.IsInBowl = true;
        Egg.EnableDrag(false);

        Egg.OnEnded += OnEggEnd;
    }

    void OnEggEnd()
    {
        Egg.OnEnded -= OnEggEnd;
        Reset();
    }

    private void SavePosition()
    {
        _lastPosition = transform.position;
    }

    private void ResetToLastPosition()
    {
        transform.position = _lastPosition;
    }

    // Control visibility of seasoning set
    void ShowSeasoningSet(bool show)
    {
        _seasoningSet.SetActive(show);
    }

    void AddSeasoningSet()
    {
        ShowSeasoningSet(true);
        Egg.HasSeasoningSet = true;
        _dropHandler.SucceedDrop();
        EnableDropHandler(false);
    }

    void EnableDropHandler(bool enable)
    {
        _dropHandler.enabled = enable;
    }

    void Reset()
    {
        ShowSeasoningSet(false);
        Egg = null;
        EnableDropHandler(true);
    }
}
