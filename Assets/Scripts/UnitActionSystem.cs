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
    void Update()
    {
        if (isBusy) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedUnit.GetMoveAction().IsValidActionGridPos(mouseGridPosition))
            {
                SetBusy();
                selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);    
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy);
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
        {
            if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }
    
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
