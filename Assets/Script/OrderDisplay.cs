using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _toast = null;
    [SerializeField] private TextMeshProUGUI _drink = null;
    [SerializeField] private TextMeshProUGUI _egg = null;

    void Start()
    {
        BlankDisplay();
    }

    public void DisplayOrder(Order order)
    {
        _toast.text = order.Bread.name;
        _drink.text = order.Drink.name;
        _egg.text = order.Egg.name;
        order.OnOrderCompleted += OnOrderComplete;
    }

    public void OnOrderComplete(Order order)
    {
        order.OnOrderCompleted -= OnOrderComplete;
        BlankDisplay();
    }

    void BlankDisplay()
    {
        _toast.text = "---";
        _drink.text = "---";
        _egg.text = "---";
    }
}
