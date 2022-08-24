using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    State state;

    int maxShootDistance = 5;
    float stateTimer;
    
    bool canShoot;

    Unit targetUnit;

    void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (canShoot)
                {
                    Shoot();
                    canShoot = false;
                }
                break;
            case State.Cooloff:                
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    
    void NextState()
    {
        switch (state)
        {
            case State.Aiming:                
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;                
                break;
            case State.Shooting:
                state = State.Cooloff;
                float cooloffStateTime = 0.3f;
                stateTimer = cooloffStateTime;                
                break;
            case State.Cooloff:
                ActionComplete();                
                break;
        }
        
    }
    void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit,
        });
        targetUnit.GetDamage();
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);

                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > maxShootDistance)
                {
                    continue;
                }

                /*validGridPositionList.Add(testGridPosition);
                continue;*/

                if (!LevelGrid.Instance.IsOccupied(testGridPosition))
                {
                    //grid position is empty, nobody to shoot at 
                    continue;
                }
                Unit targetUnit  = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShoot = true;
    }
}
