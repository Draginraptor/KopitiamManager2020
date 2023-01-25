using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Customer : MonoBehaviour
{
    [SerializeField] private RandomPerson _randomiser = null;
    [SerializeField] private List<BreadOrder> _breadChoices = new List<BreadOrder>();
    [SerializeField] private List<DrinkOrder> _drinkChoices = new List<DrinkOrder>();
    [SerializeField] private List<EggOrder> _eggChoices = new List<EggOrder>();

    public Order CustomerOrder = null;

    // Start is called before the first frame update
    void Start()
    {
        RandomiseOrder();
        _randomiser.Randomise();
    }

    public void OnMouseDown()
    {
        if(OrderManager.Instance.AttemptAddOrder(CustomerOrder))
        {
            CustomerOrder.OnOrderCompleted += OnOrderComplete;
        }
    }

    void OnOrderComplete(Order order)
    {
        RandomiseOrder();
    }

    void RandomiseOrder()
    {
        CustomerOrder = new Order(RandomBread(), RandomDrink(), RandomEgg());
    }

    BreadOrder RandomBread()
    {
        int randomIndex = Random.Range(0, _breadChoices.Count);
        return _breadChoices[randomIndex];
    }
    DrinkOrder RandomDrink()
    {
        int randomIndex = Random.Range(0, _drinkChoices.Count);
        return _drinkChoices[randomIndex];
    }
    EggOrder RandomEgg()
    {
        int randomIndex = Random.Range(0, _eggChoices.Count);
        return _eggChoices[randomIndex];
    }
}
