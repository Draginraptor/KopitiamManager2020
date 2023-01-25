using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHandler : MonoBehaviour
{
    public System.Action<GameObject> OnDropReceived = delegate { };
    public System.Action<DropHandler> OnDropSucceeded = delegate { };
    public System.Action<DropHandler> OnDropFailed = delegate { };

    public void ReceiveDrop(GameObject gameObject)
    {
        OnDropReceived(gameObject);
    }

    public void FailDrop()
    {
        OnDropFailed(this);
    }

    public void SucceedDrop()
    {
        OnDropSucceeded(this);
    }
}
