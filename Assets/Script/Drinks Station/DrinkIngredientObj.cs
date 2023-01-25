using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrinkIngredientObj : MonoBehaviour, IPointerUpHandler
{
    public DrinkIngredient DrinkIngredient = DrinkIngredient.MiloPowder;
    private Vector3 _defaultPosition = new Vector3();

    // Resets to its spawn; used to set off drop handlers
    void Awake()
    {
        _defaultPosition = transform.position;
        GetComponent<DragHandler>().OnClickReleased += OnPointerUp;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        transform.position = _defaultPosition;
    }
}