using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// https://answers.unity.com/questions/1370601/ondrag-with-instantiated-gameobject.html
public class DragHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    // Useful events for alerting other classes in cases when their DragHandlers are not directly interacted with
    public System.Action<PointerEventData> OnClickReleased = delegate { };
    public System.Action<PointerEventData> OnDragStarted = delegate { };
    public System.Action<PointerEventData> OnDragContinued = delegate { };
    public System.Action OnDropSucceeded = delegate { };
    public System.Action OnDropFailed = delegate { };
    // Camera the object will be relative to
    [SerializeField] private Camera _relativeCamera = null;

    // Consistent distance from camera regardless of angle of camera
    private Vector3 _vectorFromCamera;
    // Vector variable for using Set over creating new vectors
    private Vector3 _updatedDragPosition = new Vector3();

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        OnClickReleased(pointerEventData);

        GameObject possibleDropTarget = GetDropTarget();
        if(possibleDropTarget == null)
        {
            OnDropFailed();
            return;
        }
        // Check for a drop handler, exit function if null
        DropHandler dropHandler = possibleDropTarget.GetComponent<DropHandler>();
        if (dropHandler == null)
        {
            OnDropFailed();
            return;
        }
        // Else subscribe to the drop succeeded/failed event and attempt the drop
        dropHandler.OnDropSucceeded += OnDropSucceed;
        dropHandler.OnDropFailed += OnDropFail;
        dropHandler.ReceiveDrop(gameObject);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        OnDragStarted(pointerEventData);
        MoveToMouse();
        _vectorFromCamera = transform.position - _relativeCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = MoveInFrontOfCamera();
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        UpdateDragPosition();
        transform.position = _updatedDragPosition;
    }

    private void OnDropFail(DropHandler dropHandler)
    {
        // Unsubscribe from event
        dropHandler.OnDropSucceeded -= OnDropSucceed;
        dropHandler.OnDropFailed -= OnDropFail;
        OnDropFailed();
    }

    private void OnDropSucceed(DropHandler dropHandler)
    {
        // Unsubscribe from event
        dropHandler.OnDropSucceeded -= OnDropSucceed;
        dropHandler.OnDropFailed -= OnDropFail;
        OnDropSucceeded();
    }

    void UpdateDragPosition()
    {
        _updatedDragPosition = _relativeCamera.ScreenToWorldPoint(Input.mousePosition) + _vectorFromCamera;
        _updatedDragPosition = MoveInFrontOfCamera();
    }

    Vector3 MoveInFrontOfCamera()
    {
        return _relativeCamera.ScreenToWorldPoint(Input.mousePosition) + _relativeCamera.transform.forward;
    }

    public void MoveToMouse()
    {
        transform.position = _relativeCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void ExternalSetup(Camera relativeCamera)
    {
        _relativeCamera = relativeCamera;
    }

    public GameObject GetDropTarget()
    {
        Ray ray = _relativeCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f, ~LayerMask.NameToLayer("DropHandlers"));

        for(int i = 0; i < hits.Length; i++)
        {
            GameObject drop = hits[i].collider.gameObject;
            // If object is the same as the one this script is attached to return null
            if(drop != gameObject)
            {
                return drop;
            }
        }
        // Last resort
        return null;
    }
}
