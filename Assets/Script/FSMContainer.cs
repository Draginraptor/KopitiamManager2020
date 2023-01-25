using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FSMContainer : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Camera _relativeCamera = null;
    [SerializeField] private DragHandler _fsmPrefab = null;
    [SerializeField] private Transform _spawnPoint = null;
    [SerializeField] private int _initialPoolSize = 3;

    private List<DragHandler> _fsmPool = new List<DragHandler>();
    private DragHandler _activeItem = null;
    
    // Start is called before the first frame update
    void Start()
    {
        InitialisePool();
    }

    // Click events originate from this object often, and usually needs to be
    // passed into the items they manage
    // PointerEventData is passed from this object to the next
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        _activeItem.OnPointerUp(pointerEventData);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        GetAvailableFSM();
        _activeItem.OnPointerDown(pointerEventData);
    }

    // Pass Drag events to DragHandler
    public void OnDrag(PointerEventData pointerEventData)
    {
        _activeItem.OnDrag(pointerEventData);
    }

    DragHandler CreateNewFSM()
    {
        DragHandler item = Instantiate(_fsmPrefab, _spawnPoint, false);
        item.ExternalSetup(_relativeCamera);
        _fsmPool.Add(item);
        return item;
    }
    
    void InitialisePool()
    {
        for(int i = 0; i < _initialPoolSize; i++)
        {
            CreateNewFSM();
        }
    }

    void GetAvailableFSM()
    {
        // Check for isComplete on the FSM on all existing objects
        for(int i = 0; i < _fsmPool.Count; i++)
        {
            if(CheckAtSpawn(_fsmPool[i].gameObject))
            {
                _activeItem = _fsmPool[i];
                // Exit function early if found
                return;
            }
        }
        _activeItem = CreateNewFSM();
    }

    // If the object is at the spawn location, it can be reused
    // Workaround trying to retrieve the FSM from the various gameobjects
    bool CheckAtSpawn(GameObject fsm)
    {
        return fsm.transform.position == _spawnPoint.position;
    }
}
