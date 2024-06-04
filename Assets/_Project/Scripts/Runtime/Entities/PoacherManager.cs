using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class poachpath
{
    public int2[] pathSteps;
}

public class PoacherManager : MonoBehaviour
{
    #region FIELDS

    public TileManager tileManager;
    public TileContainer tileContainer;
    public poacherContainer teamContainer;

    //public GameObject inputCanvas;
    public bool fail = false;

    public bool PoacherMove = false;
    public int teamNumber;
    public int UnitCount = 1;
    public bool isTurn = false;
    public GameObject TrapPrefab, EnemyUnitPrefab;
    public int2[] unitPositions;
    public UnitTypes[] unitTypes;
    public poachpath[] paths;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        UnitCount = unitTypes.Length;
        //_totalUnitCount = UnitCount;
    }

    private void Awake()
    {
        teamContainer.team = teamNumber;
        StartCoroutine(UnitLoad(0.5f));
    }

    private void Update()
    {
        if (UnitCount <= 0)
        {
            EventManager.PlayerWin?.Invoke(teamNumber);//death check
        }
        if (!isTurn) return;//only starts turn if it is their turn
        StartTurn();
    }

    #endregion UNITY METHODS

    #region METHODS

    public IEnumerator UnitLoad(float time)// loads unit with a small delay to prevent null reference from large loads at the start by other classes
    {
        yield return new WaitForSeconds(time);
        double t = Time.timeSinceLevelLoadAsDouble;
        for (int i = 0; i < unitPositions.Length; i++)
        {
            instantiateUnit(unitPositions[i], unitTypes[i]);
        }
        int count = 0;
        foreach (Poacher poach in teamContainer.units)
        {
            poach.PoacherPath = paths[count].pathSteps;
            poach.maxPathIndex = poach.PoacherPath.Length - 1;
            count++;
        }
    }

    public void instantiateUnit(int2 pos, UnitTypes unitType)//instantiates a unit - in a pos and of a type - will allow us to automatically load units latter
    {
        GameObject unit = null;

        Transform placementPoint = tileManager.GetTile(pos).properties.PlacementPoint;
        switch (unitType)
        {
            case UnitTypes.Rhino:

                break;

            case UnitTypes.PlayerCharacter:

                break;

            case UnitTypes.Trap:
                Trap trap = null;
                unit = Instantiate(TrapPrefab, placementPoint.position, placementPoint.rotation);
                trap = unit.GetComponent<Trap>();
                //unit.GetComponent<Poacher>().teamManager = this;
                trap.tileManager = tileManager;
                trap.poacherManager = this;
                trap.team = trap.unitProperties.team = teamNumber;

                tileContainer.PosTileDict[pos].selectable = true;

                trap.InitMove(pos);//moves the unit to the correct position
                break;

            case UnitTypes.Poacher:
                Poacher Poach = null;
                unit = Instantiate(EnemyUnitPrefab, placementPoint.position, placementPoint.rotation);
                Poach = unit.GetComponent<Poacher>();
                unit.GetComponent<Poacher>().teamManager = this;
                Poach.tileManager = tileManager;
                Poach.teamManager = this;
                Poach.team = Poach.unitProperties.team = teamNumber;
                //adds the unit to the team container
                teamContainer.AddUnit(Poach, pos);

                tileContainer.PosTileDict[pos].selectable = true;
                Poach.InitMove(pos);//moves the unit to the correct position
                break;
        }
    }

    public void StartTurn()//starts the turn
    {
        if (!PoacherMove)
        {
            foreach (Poacher poach in teamContainer.units)
            {
                //Debug.Log("Poacher Tacking turn");
                poach.TackTurn();
            }
            PoacherMove = true;
        }
        tileManager.resetTiles();
        isTurn = false;
        PoacherMove = false;
        EventManager.NextTurn?.Invoke();
    }

    public void SetUnitLock(bool lockState)
    {
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            tileContainer.PosTileDict[teamContainer.units[i].unitProperties.Pos].properties.canHover = lockState;
        }
    }

    public void SetTileSelectable(bool Selectable)
    {
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            tileContainer.PosTileDict[teamContainer.units[i].unitProperties.Pos].selectable = Selectable;
        }
    }

    public void stopTeamHover()
    {
        foreach (Tile tile in tileManager.tileContainer.tiles)
        {
            tile.StopHover();
        }
    }

    public void setTileHover(bool hoverState)
    {
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            tileContainer.PosTileDict[teamContainer.units[i].unitProperties.Pos].properties.hover = hoverState;
        }
    }

    public void setCanHover(bool hover)
    {
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            tileContainer.PosTileDict[teamContainer.units[i].unitProperties.Pos].properties.canHover = hover;
        }
    }

    #endregion METHODS
}