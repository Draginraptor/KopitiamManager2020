using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationSwapper : MonoBehaviour
{
    [SerializeField] private GameObject _ordersStation = null;
    [SerializeField] private GameObject _toastStation = null;
    [SerializeField] private GameObject _drinkStation = null;
    [SerializeField] private GameObject _eggsStation = null;

    void Start()
    {
        // Default screen
        GoToOrders();
    }

    public void GoToOrders()
    {
        _ordersStation.SetActive(true);
        _toastStation.SetActive(false);
        _drinkStation.SetActive(false);
        _eggsStation.SetActive(false);

    }

    public void GoToToast()
    {
        _toastStation.SetActive(true);
        _ordersStation.SetActive(false);
        _drinkStation.SetActive(false);
        _eggsStation.SetActive(false);
    }

    public void GoToDrinks()
    {
        _drinkStation.SetActive(true);
        _ordersStation.SetActive(false);
        _toastStation.SetActive(false);
        _eggsStation.SetActive(false);
    }

    public void GoToEggs()
    {
        _eggsStation.SetActive(true);
        _ordersStation.SetActive(false);
        _toastStation.SetActive(false);
        _drinkStation.SetActive(false);
    }
}
