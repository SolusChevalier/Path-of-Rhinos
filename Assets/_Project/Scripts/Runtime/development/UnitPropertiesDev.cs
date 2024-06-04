using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class UnitPropertiesDev : MonoBehaviour
{
    #region FIELDS

    public UnitTypes unitType;

    public int UnitBaseValue;
    public int health;
    public int maxHealth = 100;
    public int attack;
    public int defense;
    public int attackRange;
    public int movementRange;
    public int team;
    public int2 Pos;
    public UnityEvent OnDeath;

    #endregion FIELDS

    #region UNITY METHODS

    private void Awake()
    {
        onStart();
    }

    #endregion UNITY METHODS

    #region METHODS

    public void onStart()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)//deals damage to the unit
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);  // Ensure health stays within bounds
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()//handles the death of the unit
    {
        OnDeath?.Invoke();//invoke the death event
    }

    #endregion METHODS
}