using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    // this is where we will generate the map but for now it just stores the tiles in a manually ordered list
    public List<Tile> tiles = new List<Tile>();

    // this is a dictionary that will allow us to access the tiles by their position
    public Dictionary<int2, Tile> PosTileDict = new Dictionary<int2, Tile>();

    private void Start()
    {
        //loads the tiles into the dictionary
        int count = 0;
        for (int Y = 0; Y <= 15; Y++)
        {
            for (int X = 0; X <= 31; X++)
            {
                PosTileDict.Add(new int2(X, Y), tiles[count]);
                count++;
            }
        }

        /*
        Tile[] tilesArray = tiles.ToArray();
        var job = new DictLoad
        {
            Input = tilesArray,
            Output = PosTileDict
        };
        job.Schedule().Complete();
        */
    }

    // this function will return the tile at the given position
    public int2 KeyByValue(Tile value)
    {
        foreach (KeyValuePair<int2, Tile> entry in PosTileDict)
        {
            if (entry.Value == value)
            {
                return entry.Key;
            }
        }
        return new int2(-1, -1);
    }

    public void ResetTileSelectable()
    {
        foreach (Tile tile in tiles)
        {
            tile.selectable = false;
        }
    }

    /*[BurstCompile(CompileSynchronously = true)]
    private struct DictLoad : IJob
    {
        [ReadOnly]
        public Tile[] Input;

        [WriteOnly]
        public Dictionary<int2, Tile> Output;

        public void Execute()
        {
            int count = 0;
            for (int Y = 0; Y <= 7; Y++)
            {
                for (int X = 0; X <= 15; X++)
                {
                    Output.Add(new int2(X, Y), Input[count]);
                    count++;
                }
            }
        }
    }*/
}