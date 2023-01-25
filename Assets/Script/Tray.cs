using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour
{
    [SerializeField] private DropHandler _dropHandler = null;

    private EggState _requiredEggBoiledLevel = EggState.HalfBoiled;
    private DrinkType _requiredDrink = DrinkType.Water;
    private BreadType _requiredBread = BreadType.ButterToast;

    private bool _isEggFulfilled = false;
    private bool _isDrinkFulfilled = false;
    private bool _isBreadFulfilled = false;

    private Order _order = null;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to DropHandler events to run code accordingly
        _dropHandler.OnDropReceived += CheckReceivedDrop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckReceivedDrop(GameObject drop)
    {
        // Check for each drop, exiting function early if any is identified
        if(CheckForBread(drop))
        {
            CheckOrderCompletion();
            return;
        }
        else if(CheckForEggBowl(drop))
        {
            CheckOrderCompletion();
            return;
        }
        else if(CheckForDrink(drop))
        {
            CheckOrderCompletion();
            return;
        }
        // Penalise for incorrect order
        Scoreboard.Instance.ChangeScore(-10);
        // Look for the EggFSM component
        _dropHandler.FailDrop();
    }

    bool CheckForBread(GameObject drop)
    {
        // If bread part of the order is already done, exit early
        if(_isBreadFulfilled) { return false; }
        Plate plate = drop.GetComponent<Plate>();
        if(plate != null)
        {
            if(IsBreadApplicable(plate.Bread))
            {
                plate.Bread.IsPlacedOnTray = true;
                _isBreadFulfilled = true;
                _dropHandler.SucceedDrop();
                return true;
            }
        }
        return false;
    }

    bool IsBreadApplicable(BreadFSM bread)
    {
        if(bread == null) { return false; }
        return bread.FinalBreadType == _requiredBread && bread.CurrentState.GetName() == BreadState.Ready.ToString();
    }

    bool CheckForEggBowl(GameObject drop)
    {
        // If egg part of the order is already done, exit early
        if(_isEggFulfilled) { return false; }
        Bowl bowl = drop.GetComponent<Bowl>();
        if(bowl != null)
        {
            if(IsEggApplicable(bowl.Egg))
            {
                bowl.Egg.IsPlacedOnTray = true;
                _isEggFulfilled = true;
                _dropHandler.SucceedDrop();
                return true;
            }
        }
        return false;
    }

    bool IsEggApplicable(EggFSM egg)
    {
        if(egg == null) { return false; }
        return egg.EggBoiledLevel == _requiredEggBoiledLevel && egg.CurrentState.GetName() == EggState.Ready.ToString();
    }

    bool CheckForDrink(GameObject drop)
    {
        // If drink part of the order is already done, exit early
        if(_isDrinkFulfilled) { return false; }
        DrinkFSM drink = drop.GetComponent<DrinkFSM>();
        if(drink != null)
        {
            if(IsDrinkApplicable(drink))
            {
                drink.IsPlacedOnTray = true;
                _isDrinkFulfilled = true;
                _dropHandler.SucceedDrop();
                return true;
            }
        }
        return false;
    }

    bool IsDrinkApplicable(DrinkFSM drink)
    {
        if(drink == null) { return false; }
        return drink.FinalDrinkType == _requiredDrink && drink.CurrentState.GetName() == DrinkState.Ready.ToString();
    }

    private void CheckOrderCompletion()
    {
        if(_isBreadFulfilled && _isDrinkFulfilled && _isEggFulfilled)
        {
            _order.CompleteOrder();
            _order = null;
            Scoreboard.Instance.ChangeScore(100);
            gameObject.SetActive(false);
        }
    }

    public bool IsFree()
    {
        return _order == null;
    }

    public void AddOrder(Order order)
    {
        _order = order;
        _requiredBread = order.Bread.breadType;
        _requiredDrink = order.Drink.drinkType;
        _requiredEggBoiledLevel = order.Egg.eggType;
    }
    
}
