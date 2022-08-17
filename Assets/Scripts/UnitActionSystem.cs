using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChange;


    [SerializeField] Unit selectedUnit;
    [SerializeField] LayerMask unitLayerMask;

    BaseAction selectedAction;
    bool isBusy;

    void Awake()
    {        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    void Update()
    {
        if (isBusy) return;
               
        if (TryHandleUnitSelection()) return;

        HandleSelectedAction();
    }

    void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            switch (selectedAction)
            {
                case MoveAction moveAction:
                    if (moveAction.IsValidActionGridPos(mouseGridPosition))
                    {
                        SetBusy();
                        moveAction.Move(mouseGridPosition, ClearBusy);
                    }
                    break;

                case SpinAction spinAction:
                    SetBusy();                    
                    spinAction.Spin(ClearBusy);
                    break;
            }
        }
    }

    void SetBusy()
    {
        isBusy = true;
    }
    void ClearBusy()
    {
        isBusy = false;
    }

    bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        SetSelectedAction(unit.GetMoveAction());

        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
