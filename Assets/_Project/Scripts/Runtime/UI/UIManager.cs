using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    //simple ui manager
    public GameObject playerCan;

    public TeamManager TeamManager;
    public PoacherManager poachManager;
    public TextMeshProUGUI TeamText;
    public float Score;

    public void Start()
    {
        TeamText.color = Color.red;

        TeamText.text = "Score: " + TeamManager.GetTeamValue() + "\nUnits: " + TeamManager.UnitCount;
    }

    private void Update()
    {
        if (TeamManager.isTurn)
        {
            playerCan.SetActive(true);
        }
        if (poachManager.isTurn)
        {
            playerCan.SetActive(false);
        }
        TeamText.text = "Score: " + TeamManager.GetTeamValue() + "\nUnits: " + TeamManager.UnitCount;
    }
}