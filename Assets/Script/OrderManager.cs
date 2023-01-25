using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance = null;
    [SerializeField] private Transform _toastStationTransform = null;
    [SerializeField] private Transform _drinksStationTransform = null;
    [SerializeField] private Transform _eggsStationTransform = null;

    [SerializeField] private List<OrderDisplayAndTrayCombo> _orderDisplaysAndTrays = new List<OrderDisplayAndTrayCombo>();

    [System.Serializable]
    public struct OrderDisplayAndTrayCombo
    {
        public OrderDisplay orderDisplay;
        public Tray tray;
    }

    private List<Order> _orders = new List<Order>();

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AttemptAddOrder(Order order)
    {
        if(_orders.Count >= 3 || _orders.Contains(order))
        {
            return false;
        }
        _orders.Add(order);
        order.OnOrderCompleted += OnOrderComplete;
        OrderDisplayAndTrayCombo combo = GetAvailableTrayCombo();
        if(!combo.tray.IsFree()) { return false; }
        // Display order on Orders station
        combo.orderDisplay.DisplayOrder(order);
        // Setup a tray
        combo.tray.gameObject.SetActive(true);
        combo.tray.AddOrder(order);
        return true;
    }

    public void OnOrderComplete(Order order)
    {
        order.OnOrderCompleted -= OnOrderComplete;
        _orders.Remove(order);
    }

    public void MoveToToastStation()
    {
        transform.position = _toastStationTransform.position;
    }

    public void MoveToDrinksStation()
    {
        transform.position = _drinksStationTransform.position;
    }

    public void MoveToEggsStation()
    {
        transform.position = _eggsStationTransform.position;
    }

    OrderDisplayAndTrayCombo GetAvailableTrayCombo()
    {
        for(int i = 0; i < _orderDisplaysAndTrays.Count; i++)
        {
            if(_orderDisplaysAndTrays[i].tray.IsFree())
            {
                return _orderDisplaysAndTrays[i];
            }
        }

        return _orderDisplaysAndTrays[0];
    }
}
