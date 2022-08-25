using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChange;
    public event EventHandler OnDead;
    [SerializeField] int health = 100;

    int healthMax;

    private void Awake()
    {
        healthMax = health;
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }
        OnHealthChange?.Invoke(this, EventArgs.Empty);

        if (health == 0)
        {
            Die();
        }
    }
    void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
