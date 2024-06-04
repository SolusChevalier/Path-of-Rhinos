using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum GameState
{
    Player1Turn,
    Player2Turn,
    Player1Won,
    Player2Won,
    paused
}