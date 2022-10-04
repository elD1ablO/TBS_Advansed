using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    [SerializeField] Transform crateDestroyedPrefab;
    public static event EventHandler OnAnyDestroyed;

    GridPosition gridPositon;

    void Start()
    {
        gridPositon = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetCratePosition()
    {
        return gridPositon;
    }
    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);
        /*gameObject.SetActive(false);

        GameObject crateParts = crateDestroyedTransform.GetComponent<GameObject>();
        Destroy(crateParts,2f);*/

        Destroy(gameObject);        
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty); 
    }


    void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);                
            }
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
  
}
