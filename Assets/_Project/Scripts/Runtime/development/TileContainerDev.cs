using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class TileContainerDev
{
    #region FIELDS

    // this is where we will generate the map but for now it just stores the tiles in a manually ordered list
    public List<TileDev> tiles = new List<TileDev>();

    // this is a dictionary that will allow us to access the tiles by their position
    public Dictionary<int2, int> Position_TileHash_Dict = new Dictionary<int2, int>();

    public Dictionary<int, int2> TileHash_Position_Dict = new Dictionary<int, int2>();

    public Dictionary<int, TileDev> Tile_TileHash_Dict = new Dictionary<int, TileDev>();
    private TileDev _HoverTile;

    #endregion FIELDS

    #region METHODS

    public TileContainerDev(List<TileDev> StartTiles)
    {
        tiles = StartTiles;
        Tile_TileHash_Dict = StartTiles.ToDictionary(tile => tile.GetHashCode());
        int count = 0;
        for (int Y = 0; Y <= 7; Y++)
        {
            for (int X = 0; X <= 15; X++)
            {
                Position_TileHash_Dict.Add(new int2(X, Y), tiles[count].GetHashCode());
                TileHash_Position_Dict.Add(tiles[count].GetHashCode(), new int2(X, Y));
                count++;
            }
        }
    }

    public void MouseOverTile(TileDev tile)
    {
        if (tile != null)
        {
            if (_HoverTile == null)
            {
                _HoverTile = tile;
                _HoverTile.HoverOver();
            }
            else
            {
                _HoverTile.EndHoverOver();
                _HoverTile = tile;
                _HoverTile.HoverOver();
            }
        }
        else
        {
            if (_HoverTile != null)
            {
                _HoverTile.EndHoverOver();
                _HoverTile = null;
            }
        }
    }

    #region BOILERPLATE

    public void SetTilesSelectionState(TileDev[] tiles, SelectionState state)
    {
        foreach (TileDev tile in tiles)
        {
            tile.SetSelectionSate(state);
        }
    }

    public void SetTilesSelectionState(int[] hashes, SelectionState state)
    {
        foreach (int hash in hashes)
        {
            Tile_TileHash_Dict[hash].SetSelectionSate(state);
        }
    }

    public void SetTilesSelectionState(int2[] Positions, SelectionState state)
    {
        foreach (int2 pos in Positions)
        {
            Tile_TileHash_Dict[Position_TileHash_Dict[pos]].SetSelectionSate(state);
        }
    }

    public void SetTileSelectionState(int2 Pos, SelectionState state)
    {
        Tile_TileHash_Dict[Position_TileHash_Dict[Pos]].SetSelectionSate(state);
    }

    public void SetTileSelectionState(int Hash, SelectionState state)
    {
        Tile_TileHash_Dict[Hash].SetSelectionSate(state);
    }

    public void SetTileSelectionState(TileDev tile, SelectionState state)
    {
        tile.SetSelectionSate(state);
    }

    public void ResetTileSelectable()
    {
        foreach (TileDev tile in tiles)
        {
            tile.SetSelectionSate(SelectionState.Inert);
        }
    }

    public TileDev GetTileFromPosition(int2 Pos)
    {
        return Tile_TileHash_Dict[Position_TileHash_Dict[Pos]];
    }

    public int2 GetPositionFromTileHash(int Hash)
    {
        return TileHash_Position_Dict[Hash];
    }

    #endregion BOILERPLATE

    /*[BurstCompile(CompileSynchronously = true)]
    private struct DictLoad : IJob
    {
        [ReadOnly]
        public NativeArray<int> TileHash;

        [WriteOnly]
        public Dictionary<int2, int> PosTileDict;

        public Dictionary<int, int2> TilePosDict;

        public void Execute()
        {
            int count = 0;
            for (int Y = 0; Y <= 7; Y++)
            {
                for (int X = 0; X <= 15; X++)
                {
                    PosTileDict.Add(new int2(X, Y), TileHash[count]);
                    TilePosDict.Add(TileHash[count], new int2(X, Y));
                    count++;
                }
            }
        }
    }*/

    #endregion METHODS
}