using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UnitProperties : MonoBehaviour
{
    #region FIELDS

    //this is the class that stors all the units properties
    //we can use this later in the project
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
    public UnityEvent OnDied;

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
        OnDied?.Invoke();//invoke the death event
    }

    #endregion METHODS
}