using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] Unit unit;
    

    MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnityActionSystem_OnSelectedUnitChange;

        UpdateVisual();
    }

    void UnityActionSystem_OnSelectedUnitChange(object sender, EventArgs Empty)
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange -= UnityActionSystem_OnSelectedUnitChange;
    }
}
