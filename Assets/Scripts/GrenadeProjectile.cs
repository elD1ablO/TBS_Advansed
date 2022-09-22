using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] int damageAmount = 30;
    [SerializeField] Transform GrenadeExplodeVFXPrefab;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] AnimationCurve arcYAnimationCurve;

    public static event EventHandler OnAnyGrenadeExploded;

    private Vector3 targetPosition;

    Vector3 positionXZ;
    float totalDistance;
    Action onGrenadeBehaviorComplete;

    void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        float moveSpeed = 15f;

        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance /4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = .2f;

        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
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
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;
            Instantiate(GrenadeExplodeVFXPrefab, targetPosition + (Vector3.up * 0.5f), Quaternion.identity);

            Destroy(gameObject);

            onGrenadeBehaviorComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        this.onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
