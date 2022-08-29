using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    List<Unit> unitList;
    List<Unit> playersUnitList;
    List<Unit> enemyUnitList;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new();
        playersUnitList = new();
        enemyUnitList = new();

        
    }

    
    void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Add(unit);

        if (unit.IsEnemy())
        {            
            enemyUnitList.Add(unit);
        }
        else
        {
            playersUnitList.Add(unit);
        }
    }

    void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Remove(unit);

        if (unit.IsEnemy())
        {            
            enemyUnitList.Remove(unit);
        }
        else
        {
            playersUnitList.Remove(unit);
        }
    }
    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetPlayersUnitList()
    {
        return  playersUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
}
