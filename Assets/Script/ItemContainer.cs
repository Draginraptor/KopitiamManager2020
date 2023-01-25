using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemContainer : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Camera _relativeCamera = null;
    [SerializeField] private DragHandler _item = null;

    void Start()
    {
        _item.ExternalSetup(_relativeCamera);
    }

    // Click events originate from this object often, and usually needs to be
    // passed into the items they manage
    // PointerEventData is passed from this object to the next
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        _item.OnPointerUp(pointerEventData);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        _item.OnPointerDown(pointerEventData);
    }

    // Pass Drag events to DragHandler
    public void OnDrag(PointerEventData pointerEventData)
    {
        _item.OnDrag(pointerEventData);
    }
}
