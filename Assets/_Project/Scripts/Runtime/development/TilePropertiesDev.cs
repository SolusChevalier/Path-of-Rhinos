using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TilePropertiesDev
{
    #region FIELDS

    public Vector3 StartPos;
    public Transform PlacementPoint;
    public bool Occupied;
    public Unit OccupyingUnit;

    #endregion FIELDS

    #region METHODS

    public TilePropertiesDev()
    {
        StartPos = Vector3.zero;
        Occupied = false;
        OccupyingUnit = null;
        PlacementPoint = null;
    }

    public TilePropertiesDev(Vector3 stPos, Transform ppPoint)
    {
        StartPos = stPos;
        Occupied = false;
        OccupyingUnit = null;
        PlacementPoint = ppPoint;
    }

    #endregion METHODS
}