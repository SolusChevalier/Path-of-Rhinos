using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class Rhino : MonoBehaviour
{
    public UnitProperties unitProperties;
    public TileManager tileManager;
    public TeamManager teamManager;
    public int2[] rhinoPath;
    public int team;
    private bool ontile = false;
    public MeshRenderer meshRenderer;
    public Color[] Colours;

    private void Awake()
    {
        unitProperties.OnDied.AddListener(HandleUnitDeath);
        meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
        Colours = new Color[meshRenderer.materials.Length];
        int i = 0;
        foreach (var mat in meshRenderer.materials)
        {
            Colours[i] = mat.color;
            i++;
        }
    }

    private void Update()
    {
        if (!ontile)//moves the unit with the tile to avoid clipping
        {
            Tile tile = tileManager.GetTile(unitProperties.Pos);
            transform.position = tile.properties.PlacementPoint.position;
        }
    }

    private void HandleUnitDeath()//handles the death of the unit
    {
        tileManager.GetTile(unitProperties.Pos).properties.Occupied = false;
        tileManager.GetTile(unitProperties.Pos).properties.OccupyingUnit = null;
        teamManager.teamContainer.rhinos.Remove(this);
        teamManager.UnitCount--;
        teamManager.removeRhino(this);
        Destroy(gameObject);
    }

    public void TakeDamage(int dam, out bool complete)//deals damage to the unit
    {
        complete = true;
        StartCoroutine(MaterialChange());
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
        Tile tile = tileManager.GetTile(newPos);//grabs the tile at the new position
        if (tile.properties.Occupied)//if the tile is not selectable or is occupied
        {
            complete = false;//break the movement and out complete as false
            return;
        }

        Tile currentTile = tileManager.GetTile(unitProperties.Pos);//grabs the tile the unit is currently on
        //resets the current tile properties
        currentTile.properties.Occupied = false;
        currentTile.properties.OccupyingUnit = null;
        //sets the new tile properties
        tile.properties.Occupied = true;
        //sets the new position of the unit
        unitProperties.Pos = newPos;
        //moves the unit to the new position
        StartCoroutine(MoveUnit(tile.properties.PlacementPoint.position));
        //transform.position = Vector3.Lerp(transform.position, tile.properties.PlacementPoint.position, Time.deltaTime);
        //transform.position = tile.properties.PlacementPoint.position;
        //ontile = true;

        complete = true;
        return;
    }

    public void InitMove(int2 newPos)//moves the unit to the new position
    {
        Tile tile = tileManager.GetTile(newPos);//grabs the tile at the new position
        if (!tile.selectable | tile.properties.Occupied)//if the tile is not selectable or is occupied
        {
            return;
        }

        Tile currentTile = tileManager.GetTile(unitProperties.Pos);//grabs the tile the unit is currently on
        //resets the current tile properties
        currentTile.properties.Occupied = false;
        currentTile.properties.OccupyingUnit = null;
        //sets the new tile properties
        tile.properties.Occupied = true;
        //sets the new position of the unit
        unitProperties.Pos = newPos;
        //moves the unit to the new position
        transform.position = tile.properties.PlacementPoint.position;
        ontile = true;

        return;
    }

    private IEnumerator MoveUnit(Vector3 target)
    {
        ontile = false;

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 5f);
            yield return null;
        }

        transform.position = target;
        ontile = true;
    }

    private IEnumerator MaterialChange()
    {
        var mats = this.gameObject.GetComponentInChildren<MeshRenderer>().materials;
        int i = 0;
        foreach (var mat in mats)
        {
            mat.color = Color.red;
            i++;
        }
        yield return new WaitForSeconds(0.25f);
        i = 0;
        foreach (var mat in mats)
        {
            mat.color = Colours[i];
            i++;
        }
    }
}