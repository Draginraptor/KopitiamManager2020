using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public System.Action<Order> OnOrderCompleted = delegate { };
    public BreadOrder Bread;
    public DrinkOrder Drink;
    public EggOrder Egg;

    public Order(BreadOrder bread, DrinkOrder drink, EggOrder egg)
    {
        Bread = bread;
        Drink = drink;
        Egg = egg;
    }

    public void CompleteOrder()
    {
        OnOrderCompleted(this);
    }
}

[System.Serializable]
public struct BreadOrder
{
    public BreadType breadType;
    public string name;
}

[System.Serializable]
public struct DrinkOrder
{
    public DrinkType drinkType;
    public string name;
}

[System.Serializable]
public struct EggOrder
{
    public EggState eggType;
    public string name;
}
