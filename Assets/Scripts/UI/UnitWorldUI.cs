using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI actionPointsText;
    [SerializeField] Unit unit;
    [SerializeField] Image healthBarImage;
    [SerializeField] HealthSystem healthSystem;


    void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnHealthChange += HealthSystem_OnHealthChange;
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    void HealthSystem_OnHealthChange(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
    void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
