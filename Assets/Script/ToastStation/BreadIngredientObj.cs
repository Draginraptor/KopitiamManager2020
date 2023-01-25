using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Used to set off drop handlers
public class BreadIngredientObj : MonoBehaviour, IPointerUpHandler
{
    public BreadIngredient BreadIngredient = BreadIngredient.Butter;
    private Vector3 _defaultPosition = new Vector3();

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