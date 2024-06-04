using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitDev : MonoBehaviour
{
    #region FIELDS

    public UnitPropertiesDev unitProperties;
    public TileManagerDev tileManager;
    public UnitManagerDev teamManager;
    public int team;
    private bool ontile = false;

    #endregion FIELDS

    #region UNITY METHODS

    private void Awake()
    {
        unitProperties.OnDeath.AddListener(HandleUnitDeath);
    }

    private void Update()
    {
        if (ontile)//moves the unit with the tile to avoid clipping
        {
            //Tile tile = tileManager.GetTile(unitProperties.Pos);
            //transform.position = tile.properties.PlacementPoint.position;
        }
    }

    #endregion UNITY METHODS

    #region METHODS

    private void HandleUnitDeath()//handles the death of the unit
    {
        //tileManager.GetTile(unitProperties.Pos).properties.Occupied = false;
        //tileManager.GetTile(unitProperties.Pos).properties.OccupyingUnit = null;
        //teamManager.teamContainer.units.Remove(this);
        teamManager.UnitCount--;
        Destroy(gameObject);
    }

    public void TakeDamage(int dam, out bool complete)//deals damage to the unit
    {
        complete = true;
        unitProperties.TakeDamage(dam);
    }

    public float GetUnitValue()//gets this units current value
    {
        if (unitProperties.health <= 0)
        {
            return 0;
        }
        float value = unitProperties.health / unitProperties.maxHealth;//indecates how damaged the unit is which will reduce its value
        value *= unitProperties.UnitBaseValue;//multiplies the value by the base value of the unit
        return value;
    }

    public void Move(int2 newPos, out bool complete)//moves the unit to the new position
    {
        /*Tile tile = tileManager.GetTile(newPos);//grabs the tile at the new position
        if (!tile.selectable | tile.properties.Occupied)//if the tile is not selectable or is occupied
        {
            complete = false;//break the movement and out complete as false
            return;
        }*/

        //Tile currentTile = tileManager.GetTile(unitProperties.Pos);//grabs the tile the unit is currently on
        //resets the current tile properties
        //currentTile.properties.Occupied = false;
        //currentTile.properties.OccupyingUnit = null;
        //sets the new tile properties
        //tile.properties.Occupied = true;
        //tile.properties.OccupyingUnit = this;
        //sets the new position of the unit
        //unitProperties.Pos = newPos;
        //transform.position = tile.properties.PlacementPoint.position;
        //ontile = true;
        complete = true;
        return;
    }

    #endregion METHODS
}