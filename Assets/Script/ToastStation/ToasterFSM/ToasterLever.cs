using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Simple accessory to the Toaster FSM that alerts it with an event
public class ToasterLever : MonoBehaviour, IPointerDownHandler
{
    public System.Action OnLeverTapped = delegate { };
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        OnLeverTapped();
    }
}
