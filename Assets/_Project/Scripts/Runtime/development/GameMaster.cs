using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    #region FIELDS

    //game manager class - this controls game state and turn order
    public int TeamTurn;

    public UnitManagerDev Team1, Team2;

    public string Win1, Win2;

    public UnitContainerDev unitContainer1, unitContainer2;
    public TileManagerDev tileManager;
    //public TileContainer tileContainer;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()//subscribes to events
    {
        EventManager.NextTurn += NextTurn;
        EventManager.PlayerWin += PlayerWin;
    }

    private void OnDestroy()//kills events on destroy
    {
        EventManager.NextTurn -= NextTurn;
        EventManager.PlayerWin -= PlayerWin;
    }

    private void Awake()//sets up the game - makes team 1 go first
    {
        Team1.isTurn = true;
        Team2.isTurn = false;
        Team1.SetUnitLock(true);
        Team1.SetTileSelectable(true);
        Team2.SetUnitLock(false);
        Player1Turn();
    }

    #endregion UNITY METHODS

    #region METHODS

    public int teamPlayer()//returns the team that is currently playing
    {
        if (Team1.isTurn)
            return 1;
        else if (Team2.isTurn)
            return 2;
        return 0;
    }

    //sets the team that can be selected
    public void SetTileSelectable(int team)
    {
        if (team == 1)
        {
            Team2.setTileHover(false);
            Team2.stopTeamHover();
            Team1.SetTileSelectable(true);
            Team1.setCanHover(true);
            Team2.setTileHover(true);
        }
        else if (team == 2)
        {
            Team1.setTileHover(false);
            Team1.stopTeamHover();
            Team2.SetTileSelectable(true);
            Team2.setCanHover(true);
            Team1.setTileHover(true);
        }
    }

    //function that is called by a team manager to pass on to the next turn
    public void NextTurn()
    {
        if (TeamTurn == 1)
            Player1Turn();
        else if (TeamTurn == 2)
            Player2Turn();
    }

    //function that is called by a team manager to end the game
    public void PlayerWin(int team)
    {
        if (team == 1)
            SceneManager.LoadScene(Win1);
        if (team == 2)
            SceneManager.LoadScene(Win2);
    }

    //function that sets the turn to player 1
    private void Player1Turn()
    {
        Team1.isTurn = true;
        Team2.isTurn = false;
        TeamTurn = 2;
        //tileContainer.ResetTileSelectable();
        Team1.SetUnitLock(true);
        Team1.SetTileSelectable(true);
        Team2.SetUnitLock(false);
        Team2.SetTileSelectable(false);
    }

    //function that sets the turn to player 2
    private void Player2Turn()
    {
        Team1.isTurn = false;
        Team2.isTurn = true;
        TeamTurn = 1;
        //tileContainer.ResetTileSelectable();
        Team1.SetUnitLock(false);
        Team1.SetTileSelectable(false);
        Team2.SetUnitLock(true);
        Team2.SetTileSelectable(true);
    }

    #endregion METHODS
}