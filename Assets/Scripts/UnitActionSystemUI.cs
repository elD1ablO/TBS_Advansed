using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform actionButtonPrefab;
    [SerializeField] Transform actionButtonContainerTransform;
    
    List<ActionButtonUI> actionButtonUIList;

    void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;

        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }


    void CreateUnitActionButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {

            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            
            actionButtonUIList.Add(actionButtonUI);
        }
    }
    void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }
    void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }        
    }
}
