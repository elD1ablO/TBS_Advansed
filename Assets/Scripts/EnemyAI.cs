using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    float timer;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChange;
    }
    void Update()
    {

        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }

    void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        timer = 2;
    }
}
