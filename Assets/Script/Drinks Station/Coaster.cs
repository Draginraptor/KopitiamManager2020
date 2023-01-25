using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coaster : MonoBehaviour
{
    [SerializeField] private DropHandler _dropHandler = null;
    [SerializeField] private Collider _collider = null;
    [SerializeField] private Transform _cupTransform = null;
    [HideInInspector] public DrinkFSM Drink = null;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to DropHandler events to run code accordingly
        _dropHandler.OnDropReceived += CheckReceivedDrop;
    }

    // Checks if the drop received is relevant to the object by searching for the relevant components
    void CheckReceivedDrop(GameObject drop)
    {
        // If theres no drink check for it
        if(Drink == null)
        {
            // Look for an DrinkFSM component
            DrinkFSM drink = drop.GetComponent<DrinkFSM>();
            if(drink != null)
            {
                if(!drink.IsComplete())
                {
                    if(drink.CurrentState.GetName() == DrinkState.Empty.ToString() ||
                    drink.CurrentState.GetName() == DrinkState.AddingIngredients.ToString()||
                    drink.CurrentState.GetName() == DrinkState.Filling.ToString())
                    {
                        Drink = drink;
                        _dropHandler.SucceedDrop();
                        OnDrinkAdded();
                        return;
                    }
                }
            }
        }
        else
        {
            // Check for spoon or ingredient
            Spoon spoon = drop.GetComponent<Spoon>();
            DrinkIngredientObj ingredient = drop.GetComponent<DrinkIngredientObj>();
            if(spoon != null || ingredient != null)
            {
                Drink.DropHandler.ReceiveDrop(drop);
                return;
            }
        }
        _dropHandler.FailDrop();
    }

    void OnDrinkAdded()
    {
        Drink.transform.SetParent(transform);
        Drink.transform.position = _cupTransform.position;
        Drink.IsOnCoaster = true;

        Drink.OnDropSucceeded += OnDrinkRemoved;
        Drink.OnEnded += OnDrinkEnd;
    }

    void OnDrinkRemoved()
    {
        Drink.IsOnCoaster = false;
        Drink.OnDropSucceeded -= OnDrinkRemoved;
        Drink.OnEnded -= OnDrinkEnd;
        Reset();
    }

    void OnDrinkEnd()
    {
        Drink.OnDropSucceeded -= OnDrinkRemoved;
        Drink.OnEnded += OnDrinkEnd;
        Reset();
    }

    void EnableDropHandler(bool enable)
    {
        _dropHandler.enabled = enable;
        _collider.enabled = enable;
    }

    void Reset()
    {
        Drink = null;
        EnableDropHandler(true);
    }
}
