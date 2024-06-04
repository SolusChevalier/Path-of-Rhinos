using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public TileContainer tileContainer;
    public GameManager gameManager;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    public Tile selectedTile, TargetTile;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Awake()
    {
        StartCoroutine(WaitReset(0.75f));//waits untill the game is ready to reset the tiles
    }

    private void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);//creates a ray from the camera to the mouse position
        foreach (Tile tile in tileContainer.tiles)//loops through all the tiles
        {
            if (!tile.selectable)//if the tile is not selectable
            {
                tile.StopHover();//stop the hover
            }
            else//if the tile is selectable
            {
                tile.Hover();//start the hover
            }
        }

        if (Physics.Raycast(ray, out hit))//if the ray hits something
        {
            if (hit.collider.CompareTag("Tile"))//if the thing hit is a tile
            {
                Tile tile = hit.collider.GetComponent<Tile>();//grabs the tile that was hit
                if (Input.GetButtonDown("Fire1") & tile.selectable)//if the left mouse button is clicked and the tile is selectable
                {
                    if (selectedTile == null)//if there is no selected tile then select the tile
                    {
                        selectedTile = tile;
                        tile.Select();
                    }
                    else if (selectedTile == tile)//if the selected tile is the same as the clicked tile then deselect the tile
                    {
                        selectedTile = null;
                        tile.Select();
                        resetTiles();
                        if (TargetTile != null)//if there is a target tile then deselect it
                        {
                            TargetTile.Select();
                            TargetTile = null;
                        }
                    }
                    else//if the selected tile is not the same as the clicked tile set the clicked one as the target tile
                    {
                        if (TargetTile != null)//if there is a target tile then deselect it
                        {
                            TargetTile.Select();
                            TargetTile = null;
                        }
                        //TargetTile.Select();
                        TargetTile = tile;
                        tile.Select();
                    }
                }
                if (!tile.properties.hover || tile.selectable)//if the tile is not hovered or the tile is selectable
                {
                    tile.Hover();//start the hover
                }
            }
        }
        if (Input.GetButtonDown("Fire2"))//if the right mouse button is clicked
        {
            resetTiles();//reset all tiles
        }
    }

    //this function will reset all tiles to their default state
    public void resetTiles()
    {
        foreach (Tile tile in tileContainer.tiles)
        {
            tile.StopHover();
        }
        selectedTile = null;
        TargetTile = null;
        gameManager.SetTileSelectable(gameManager.teamPlayer());
    }

    //this function gets tiles
    public Tile GetTile(int2 pos)
    {
        return tileContainer.PosTileDict[pos];
    }

    //this function will highlight the tiles in a radius around the given position
    //the movAtt bool will determine if the tiles are highlighted for movement or attack
    //if movAtt is true, the tiles will be highlighted that the unit can move to, which are the unoccupied tiles
    //if movAtt is false, the tiles will be highlighted that the unit can attack, which are the occupied tiles
    public void PopTilesInRad(int2 PiecePos, int rad, bool MovAtt)
    {
        int2[] selectableTiles = new int2[(rad * 2 + 1) * (rad * 2 + 1)];
        for (int i = -rad; i <= rad; i++)
        {
            for (int j = -rad; j <= rad; j++)
            {
                int2 newPos = new int2(PiecePos.x + i, PiecePos.y + j);
                if (tileContainer.PosTileDict.ContainsKey(newPos))
                {
                    if (MovAtt)
                    {
                        if (!tileContainer.PosTileDict[newPos].properties.Occupied)
                        {
                            tileContainer.PosTileDict[newPos].selectable = true;
                        }
                    }
                    else
                    {
                        if (tileContainer.PosTileDict[newPos].properties.Occupied) // & tileContainer.PosTileDict[newPos].properties.OccupyingUnit.team != team
                        {
                            if (tileContainer.PosTileDict[newPos].properties.OccupyingUnit.team != gameManager.teamPlayer())
                            {
                                tileContainer.PosTileDict[newPos].selectable = true;
                                tileContainer.PosTileDict[newPos].properties.canHover = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public IEnumerator WaitReset(float time)
    {
        yield return new WaitForSeconds(time);
        resetTiles();
    }
}