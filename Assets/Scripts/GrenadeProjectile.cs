using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] int damageAmount = 30;

    private Vector3 targetPosition;
    Action onGrenadeBehaviorComplete;
    void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float moveSpeed = 15f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float reachedTargetDistance = .2f;

        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 2f;

            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(damageAmount);
                }
            }
            Destroy(gameObject);

            onGrenadeBehaviorComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        this.onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }
}
