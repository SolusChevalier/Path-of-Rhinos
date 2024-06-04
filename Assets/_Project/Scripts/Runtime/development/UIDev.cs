using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDev : MonoBehaviour
{
    #region FIELDS

    //simple ui manager
    public GameObject redCan, blueCan;

    public UnitManagerDev RedTeam, BlueTeam;
    public TextMeshProUGUI RedTeamText, BlueTeamText;
    public float redScore, blueScore;

    #endregion FIELDS

    #region UNITY METHODS

    public void Start()
    {
        RedTeamText.color = Color.red;
        BlueTeamText.color = Color.cyan;
        BlueTeamText.text = "Blue Team\nScore: " + BlueTeam.GetTeamValue() + "\nUnits: " + BlueTeam.UnitCount;
        RedTeamText.text = "Red Team\nScore: " + RedTeam.GetTeamValue() + "\nUnits: " + RedTeam.UnitCount;
    }

    private void Update()
    {
        if (RedTeam.isTurn)
        {
            redCan.SetActive(true);
            blueCan.SetActive(false);
        }
        if (BlueTeam.isTurn)
        {
            redCan.SetActive(false);
            blueCan.SetActive(true);
        }
        BlueTeamText.text = "Blue Team\nScore: " + BlueTeam.GetTeamValue() + "\nUnits: " + BlueTeam.UnitCount;
        RedTeamText.text = "Red Team\nScore: " + RedTeam.GetTeamValue() + "\nUnits: " + RedTeam.UnitCount;
    }

    #endregion UNITY METHODS
}