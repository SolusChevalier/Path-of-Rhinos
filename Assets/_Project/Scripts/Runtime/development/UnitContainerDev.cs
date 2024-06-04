using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitContainerDev : MonoBehaviour
{
    #region FIELDS

    //this is the class that stors all the units for a team
    public int team;

    //list of units
    public List<UnitDev> units = new List<UnitDev>();

    #endregion FIELDS

    #region METHODS

    //gets the total value of all the units in the team - this is the sum of all the indevidual units values
    public float GetUnitValues()
    {
        float totalValue = 0;
        foreach (var unit in units)
        {
            totalValue += unit.GetUnitValue();
        }
        return totalValue;
    }

    //adds a unit to the team
    public void AddUnit(UnitDev unit, int2 TilePos)
    {
        units.Add(unit);
        units[units.Count - 1].team = team;
    }

    #endregion METHODS
}