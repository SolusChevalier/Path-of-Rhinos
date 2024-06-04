using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TileManagerDev : MonoBehaviour
{
    #region FIELDS

    public List<TileDev> TileList = new List<TileDev>();
    private TileContainerDev _TileContainer;
    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hit;
    private TileDev _HoverTile;
    public TileDev SelectedTile, TargetTile;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        _TileContainer = new TileContainerDev(TileList);
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        _TileContainer.MouseOverTile(_HoverTile);
    }

    private void Update()
    {
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))//if the ray hits something
        {
            if (_hit.collider.CompareTag("Tile"))//if the object hit is a tile
            {
                TileDev tile = _hit.collider.GetComponent<TileDev>();//grabs the tile that was hit
                _HoverTile = tile;
                if (Input.GetButtonDown("Fire1"))//if the left mouse button is clicked - selection logic
                {
                    //break out if the tile cant be selected
                    if (tile.selectionState != SelectionState.Selectable & tile.selectionState != SelectionState.Selected) return;
                    TileClicked(tile);
                }
            }
            else
            {
                _HoverTile = null;
            }
        }
        else
        {
            _HoverTile = null;
        }
        if (Input.GetButtonDown("Fire2"))//if the right mouse button is clicked
        {
            resetTiles();//reset all tiles
        }
    }

    public void resetTiles()
    {
        _TileContainer.ResetTileSelectable();
        SelectedTile = null;
        TargetTile = null;
    }

    public void TileClicked(TileDev tile)
    {
        if (SelectedTile == null)               //if there is no selected tile then select the tile
        {
            SelectedTile = tile;
            SelectedTile.Select();
        }
        else if (SelectedTile == tile)          //if the selected tile is the same as the clicked tile then deselect the tile
        {
            SelectedTile = null;
            tile.Select();
            if (TargetTile != null)             //if there is a target tile then deselect it
            {
                TargetTile.Select();
                TargetTile = null;
            }
        }
        else                                    //if the selected tile is not the same as the clicked tile set the clicked one as the target tile
        {
            if (TargetTile != null)             //if there is a target tile then deselect it
            {
                if (tile == TargetTile)
                {
                    TargetTile.Select();
                    TargetTile = null;
                }
                else
                {
                    TargetTile.Select();
                    TargetTile = tile;
                    TargetTile.Select();
                }
            }
            else
            {
                TargetTile = tile;
                TargetTile.Select();
            }
        }
    }

    #endregion UNITY METHODS

    #region METHODS

    public void PopTilesInRad(int2 PiecePos, int rad, int team, bool MovAtt)
    {
        List<int2> selectableTiles = new List<int2>();
        for (int i = -rad; i <= rad; i++)
        {
            for (int j = -rad; j <= rad; j++)
            {
                int2 newPos = new int2(PiecePos.x + i, PiecePos.y + j);
                if (_TileContainer.Position_TileHash_Dict.ContainsKey(newPos))
                {
                    if (MovAtt)
                    {
                        if (!_TileContainer.GetTileFromPosition(newPos).properties.Occupied)
                        {
                            selectableTiles.Add(newPos);
                        }
                    }
                    else
                    {
                        if (!_TileContainer.GetTileFromPosition(newPos).properties.Occupied)
                        {
                            if (_TileContainer.GetTileFromPosition(newPos).properties.OccupyingUnit.team != team)
                            {
                                selectableTiles.Add(newPos);
                            }
                        }
                    }
                }
            }
        }
        _TileContainer.SetTilesSelectionState(selectableTiles.ToArray(), SelectionState.Selectable);
    }

    public TileDev GetTile(int2 pos)
    {
        return _TileContainer.GetTileFromPosition(pos);
    }

    #endregion METHODS
}

/*public void TileClicked(TileTest tile)
    {
        if (SelectedTile == null)//if there is no selected tile then select the tile
        {
            SelectedTile = tile;
            SelectedTile.Select();
        }
        else if (SelectedTile == tile)//if the selected tile is the same as the clicked tile then deselect the tile
        {
            SelectedTile = null;
            tile.Select();
            //tile.SetSelectionSate(SelectionState.Inert);
            //resetTiles();
            if (TargetTile != null)//if there is a target tile then deselect it
            {
                TargetTile.Select();
                //TargetTile.SetSelectionSate(SelectionState.Inert);
                TargetTile = null;
            }
        }
        else//if the selected tile is not the same as the clicked tile set the clicked one as the target tile
        {
            if (TargetTile != null)//if there is a target tile then deselect it
            {
                //TargetTile.Select();
                if (tile == TargetTile)
                {
                    //tile.SetSelectionSate(SelectionState.Inert);
                    TargetTile.Select();
                    TargetTile = null;
                }
                else
                {
                    //TargetTile.SetSelectionSate(SelectionState.Inert);
                    TargetTile.Select();
                    TargetTile = tile;
                    //TargetTile.SetSelectionSate(SelectionState.Selected);
                    TargetTile.Select();
                }
            }
            else
            {
                TargetTile = tile;
                //tile.SetSelectionSate(SelectionState.Selected);
                TargetTile.Select();
            }

            //TargetTile.Select();
            //TargetTile = tile;
            //tile.Select();
            //tile.SetSelectionSate(SelectionState.Selected);
        }
    }*/