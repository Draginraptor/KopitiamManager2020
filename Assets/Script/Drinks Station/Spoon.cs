using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// An object to display to the player to represent their action
public class Spoon : MonoBehaviour, IPointerUpHandler
{
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

