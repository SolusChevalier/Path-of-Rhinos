using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

[Serializable]
public class SceneNav : MonoBehaviour
{
    public enum Scenes
    {
        MainMenu,
        Red1Blue2,
        Red2Blue1,
        RedWin,
        BlueWin,
    }

    #region UNITY METHODS

    public void StartRandom()
    {
        var random = new System.Random();
        var scene = random.Next(1, 100);
        if (scene % 2 == 0)
        {
            LoadScene(Scenes.Red1Blue2.ToString());
        }
        else
        {
            LoadScene(Scenes.Red2Blue1.ToString());
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitApp()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.ExitPlaymode();
    }

    #endregion UNITY METHODS
}