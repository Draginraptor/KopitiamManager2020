using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plate : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private DragHandler _dragHandler = null;
    [SerializeField] private DropHandler _dropHandler = null;
    [SerializeField] private Transform _breadTransform = null;
    [HideInInspector] public BreadFSM Bread = null;

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
        // If theres no bread check for it
        if(Bread == null)
        {
            // Look for an BreadFSM component
            BreadFSM bread = drop.GetComponent<BreadFSM>();
            if(bread != null)
            {
                if(bread.CurrentState.GetName() != BreadState.Untoasted.ToString() ||
                bread.CurrentState.GetName() != BreadState.Toasted.ToString())
                {
                    Bread = bread;
                    OnBreadAdded();
                    _dropHandler.SucceedDrop();
                    return;
                }
            }
        }
        else if(Bread.CurrentState.GetName() == BreadState.Filling.ToString())
        {
            // Check for bread ingredients
            BreadIngredientObj ingredient = drop.GetComponent<BreadIngredientObj>();
            if(ingredient != null)
            {
                Bread.AddFilling(ingredient.BreadIngredient);
                return;
            }
            // Otherwise check for the other piece of bread for progressing to Cutting state
            BreadFSM bread = drop.GetComponent<BreadFSM>();
            if(bread != null)
            {
                Bread.IsCovered = true;
            }
        }
        else if(Bread.CurrentState.GetName() == BreadState.Cutting.ToString())
        {
            Knife ingredient = drop.GetComponent<Knife>();
            if(ingredient != null)
            {
                Bread.IsReady = true;
                return;
            }
        }
        _dropHandler.FailDrop();
    }

    void OnBreadAdded()
    {
        Bread.transform.SetParent(transform);
        Bread.transform.position = _breadTransform.position;
        Bread.IsOnPlate = true;
        Bread.EnableDrag(false);

        Bread.OnEnded += OnBreadEnd;
    }

    void OnBreadEnd()
    {
        Bread.OnEnded -= OnBreadEnd;
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

    void EnableDropHandler(bool enable)
    {
        _dropHandler.enabled = enable;
    }

    void Reset()
    {
        Bread = null;
        EnableDropHandler(true);
    }
}

