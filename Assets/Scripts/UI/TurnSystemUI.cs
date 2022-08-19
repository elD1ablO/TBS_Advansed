using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button endTurnButton;
    [SerializeField] TextMeshProUGUI turnNumberText;

    void Start()
    {
        endTurnButton.onClick.AddListener( () => { TurnSystem.Instance.NextTurn(); } );
        UpdateTurnText();
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    public void UpdateTurnText()
    {
        turnNumberText.text = $"Turn: {TurnSystem.Instance.GetTurnNumber()}";
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }
}
