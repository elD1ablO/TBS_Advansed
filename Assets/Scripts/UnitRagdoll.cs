using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] Transform ragdollRootBone;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);
        ApplyExplosionOnRagdoll(ragdollRootBone, 400f, transform.position, 10f);
    }

    void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    void ApplyExplosionOnRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionOnRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}