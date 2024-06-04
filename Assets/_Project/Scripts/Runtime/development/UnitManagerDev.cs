using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UnitManagerDev : MonoBehaviour
{
    #region FIELDS

    public TileManagerDev tileManager;
    public TileContainerDev tileContainer;
    public UnitContainerDev teamContainer;
    public GameMaster gameManager;
    public GameObject inputCanvas;
    public bool fail = false;
    public int teamNumber;
    public int UnitCount = 1;
    private int _totalUnitCount = 14;
    public bool isTurn = false;
    public bool prepMove = false;
    public bool prepAttack = false;
    public GameObject ArcherPrefabs, CavPrefab, SpearPrefab, SwordPrefab;
    public int2[] unitPositions;
    public UnitTypes[] unitTypes;

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
    }

    public void instantiateUnit(int2 pos, UnitTypes unitType)//instantiates a unit - in a pos and of a type - will allow us to automatically load units latter
    {
        GameObject unit = null;
        UnitDev tmpUnit = null;
        TileDev tileDev = tileManager.GetTile(pos);
        Transform placementPoint = tileDev.properties.PlacementPoint;
        switch (unitType)
        {
            case UnitTypes.Rhino:
                unit = Instantiate(ArcherPrefabs, placementPoint.position, placementPoint.rotation);
                break;

            case UnitTypes.PlayerCharacter:
                unit = Instantiate(CavPrefab, placementPoint.position, placementPoint.rotation);
                break;

            case UnitTypes.Trap:
                unit = Instantiate(SpearPrefab, placementPoint.position, placementPoint.rotation);
                break;

            case UnitTypes.Poacher:
                unit = Instantiate(SwordPrefab, placementPoint.position, placementPoint.rotation);
                break;
        }
        //sets the instantiated unit to the correct team and position
        tmpUnit = unit.GetComponent<UnitDev>();
        tmpUnit.tileManager = tileManager;
        tmpUnit.teamManager = this;
        tmpUnit.team = tmpUnit.unitProperties.team = teamNumber;
        //adds the unit to the team container
        teamContainer.AddUnit(tmpUnit, pos);

        tileDev.SetSelectionSate(SelectionState.Selectable);
        bool comp = false;
        tmpUnit.Move(pos, out comp);//moves the unit to the correct position
    }

    //gets the total value of the team
    //this is used to determine the value of the team in relation to the other team
    //we take the total value of the units in the unit container and multiply it by
    //the quotient of how many units the the team has left and the total number of units
    public float GetTeamValue()
    {
        float totalValue = teamContainer.GetUnitValues();

        if (totalValue <= 0)
        {
            return 0;
        }
        float teamValue = ((float)UnitCount / (float)_totalUnitCount);
        totalValue = (float)totalValue * (float)teamValue;
        totalValue = (float)(Math.Round(((double)totalValue), 2));

        return totalValue;
    }

    public void StartTurn()//starts the turn
    {
        //if (tileManager.selectedTile == null) return;
        //checks if there is a sellected tile
        //if (tileManager.selectedTile.properties.Occupied == false) return;
        //checks if the selected tile is occupied
        //if (tileManager.selectedTile.properties.OccupyingUnit.team != teamNumber) return;
        //checks if the unit on the selected tile is on the team

        //if there is a tile selected, it is occupied, and the unit is on the team we procede

        inputCanvas.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Keypad1) | Input.GetButtonDown("Move"))//Move
        {
            movement();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) | Input.GetButtonDown("Attack"))//Attack
        {
            attack();
        }

        if (tileManager.TargetTile == null) return;
        //checks if there is a target tile

        if (tileManager.TargetTile != null & prepAttack)//TakeAction - Attack
        {
            //int damage;
            bool complete = false;
            try//try catch to prevent null reference
            {
                //damage = tileManager.selectedTile.properties.OccupyingUnit.unitProperties.attack - tileManager.TargetTile.properties.OccupyingUnit.unitProperties.defense;
                //damage calculation
                complete = false;
                //tileManager.TargetTile.properties.OccupyingUnit.TakeDamage(damage, out complete);
                //damage taken
            }
            catch (NullReferenceException)//catches null reference - resets turn
            {
                tileManager.resetTiles();
                prepMove = false;
                prepAttack = false;
                Debug.Log("Null reference");
                return;
            }

            if (complete)//checks if the attack is complete
            {
                //if the attack is complete we reset the tiles and end the turn
                tileManager.resetTiles();
                isTurn = false;
                prepMove = false;
                prepAttack = false;
                inputCanvas.SetActive(false);
                EventManager.NextTurn?.Invoke();
            }
            else
            {
                //if the attack is not complete we reset the tiles and end the turn
                tileManager.resetTiles();
                prepMove = false;
                prepAttack = false;
            }
        }
        if (tileManager.TargetTile != null & prepMove)//TakeAction - Move
        {
            bool complete = false;
            try//try catch to prevent null reference
            {
                //moves the unit to the target tile
                //tileManager.selectedTile.properties.OccupyingUnit.Move(tileContainer.KeyByValue(tileManager.TargetTile), out complete);
            }
            catch (NullReferenceException)//catches null reference - resets turn
            {
                tileManager.resetTiles();
                prepMove = false;
                prepAttack = false;
                return;
            }

            if (complete)//checks if the move is complete
            {
                //if the move is complete we reset the tiles and end the turn
                tileManager.resetTiles();
                isTurn = false;
                prepMove = false;
                inputCanvas.SetActive(false);
                prepAttack = false;
                EventManager.NextTurn?.Invoke();
            }
            else
            {
                //if the move is not complete we reset the tiles and end the turn
                tileManager.resetTiles();
                prepMove = false;
                prepAttack = false;
            }
        }
    }

    //method to move unit
    public void movement()
    {
        //if (tileManager.selectedTile == null)//checks if you have a unit selected
        //return;
        prepMove = true;//sets the move and attack bools
        prepAttack = false;
        /*foreach (Tile tile in tileContainer.tiles)//sets all tiles to not selectable to reset them for the range check
        {
            tile.selectable = false;
        }*/
        //checks range of movement - pops up the tiles that are in range and are not occupied
        //tileManager.PopTilesInRad(tileContainer.KeyByValue(tileManager.selectedTile), tileManager.selectedTile.properties.OccupyingUnit.unitProperties.movementRange, true);
    }

    //method to attack unit
    public void attack()
    {
        /*if (tileManager.selectedTile == null)//checks if you have a unit selected
            return;*/
        prepMove = false;//sets the move and attack bools
        prepAttack = true;
        /*foreach (Tile tile in tileContainer.tiles)
        {
            tile.selectable = false;//sets all tiles to not selectable to reset them for the range check
        }*/
        //checks range of attack - pops up the tiles that are in range and are occupied by an enemy unit
        //tileManager.PopTilesInRad(tileContainer.KeyByValue(tileManager.selectedTile), tileManager.selectedTile.properties.OccupyingUnit.unitProperties.attackRange, false);
    }

    public void SetUnitLock(bool lockState)
    {
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            //tileContainer.PosTileDict[teamContainer.units[i].unitProperties.Pos].properties.canHover = lockState;
        }
    }

    public void SetTileSelectable(bool Selectable)
    {
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            //tileContainer.PosTileDict[teamContainer.units[i].unitProperties.Pos].selectable = Selectable;
        }
    }

    public void stopTeamHover()
    {
        /*foreach (Tile tile in tileManager.tileContainer.tiles)
        {
            tile.StopHover();
        }*/
    }

    public void setTileHover(bool hoverState)
    {
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            //tileContainer.PosTileDict[teamContainer.units[i].unitProperties.Pos].properties.hover = hoverState;
        }
    }

    public void setCanHover(bool hover)
    {
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            //tileContainer.PosTileDict[teamContainer.units[i].unitProperties.Pos].properties.canHover = hover;
        }
    }

    #endregion METHODS
}