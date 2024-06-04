using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public UnitProperties unitProperties;
    public TileManager tileManager;
    //public TeamManager teamManager;

    public PoacherManager poacherManager;
    public int team;

    private bool ontile = false;
    public MeshRenderer meshRenderer;
    public Color[] Colours;

    private void Awake()
    {
        var mats = this.gameObject.GetComponentInChildren<MeshRenderer>().materials;
        int i = 0;
        foreach (var mat in mats)
        {
            Color color = mat.color;
            color.a = 0.2f;
            mat.color = color;
            i++;
        }
        unitProperties.OnDied.AddListener(HandleUnitDeath);
        meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
        Colours = new Color[meshRenderer.materials.Length];
        i = 0;
        foreach (var mat in meshRenderer.materials)
        {
            Colours[i] = mat.color;
            i++;
        }
    }

    private void Update()
    {
        if (ontile)//moves the unit with the tile to avoid clipping
        {
            Tile tile = tileManager.GetTile(unitProperties.Pos);
            transform.position = tile.properties.PlacementPoint.position;
        }
        //int2[] selectableTiles = new int2[(rad * 2 + 1) * (rad * 2 + 1)];
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                int2 newPos = new int2(unitProperties.Pos.x + i, unitProperties.Pos.y + j);
                //Debug.Log(tileManager.tileContainer.PosTileDict.ContainsKey(newPos));
                if (tileManager.tileContainer.PosTileDict.ContainsKey(newPos))
                {
                    //Debug.Log(tileManager.tileContainer.PosTileDict[newPos].properties.Occupied);
                    if (tileManager.tileContainer.PosTileDict[newPos].properties.Occupied) // & tileContainer.PosTileDict[newPos].properties.OccupyingUnit.team != team
                    {
                        //Debug.Log(tileManager.tileContainer.PosTileDict[newPos].properties.OccupyingUnit == true);
                        if (tileManager.tileContainer.PosTileDict[newPos].properties.OccupyingUnit == true)
                        {
                            //Debug.Log(tileManager.tileContainer.PosTileDict[newPos].properties.OccupyingUnit.team != 2);
                            if (tileManager.tileContainer.PosTileDict[newPos].properties.OccupyingUnit.team != 2)
                            {
                                tileManager.tileContainer.PosTileDict[newPos].properties.OccupyingUnit.TakeDamage(50, out bool complete);
                                //Debug.Log("Trap triggered");
                                //Debug.Log(complete);
                                TakeDamage(100);
                            }
                        }
                    }
                }
            }
        }
    }

    private void HandleUnitDeath()//handles the death of the unit
    {
        //tileManager.GetTile(unitProperties.Pos).properties.Occupied = false;
        //tileManager.GetTile(unitProperties.Pos).properties.OccupyingUnit = null;
        //teamManager.teamContainer.units.Remove(this);
        poacherManager.UnitCount--;
        Destroy(gameObject);
    }

    public void TakeDamage(int dam)//deals damage to the unit
    {
        StartCoroutine(MaterialChange());
        HandleUnitDeath();
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
        //tile.properties.Occupied = true;
        //tile.properties.OccupyingUnit = this;
        //sets the new position of the unit
        unitProperties.Pos = newPos;
        //moves the unit to the new position
        transform.position = tile.properties.PlacementPoint.position;
        ontile = true;
        return;
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