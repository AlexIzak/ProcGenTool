using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveGen : MonoBehaviour
{
    private int width, height;

    public int seed;
    public bool useRandomSeed;

    [Range(30, 50)]
    public int randomFillPercent;

    int[,] cave;

    //TODO Add origin point so it can be set to different locations
    public void GenerateCave(int width, int height, Tilemap tilemap, TileBase hollow)
    {
        this.width = width;
        this.height = height;
        cave = new int[width, height];
        RandomFillCave();

        for (int i = 0; i < 5; i++)
        {
            SmoothCave();
        }

        DrawCave(tilemap, hollow);
    }

    private void RandomFillCave()
    {
        if (useRandomSeed)
        {
            seed = Mathf.FloorToInt(Time.time * 10f);
        }

        System.Random pseudoRandom = new System.Random(seed);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    cave[x, y] = 1;
                }
                else cave[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    void SmoothCave()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4) cave[x, y] = 1;

                else if (neighbourWallTiles < 4) cave[x, y] = 0;
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if(neighbourX >= 0 && neighbourX < width &&  neighbourY >= 0 && neighbourY < height) //Check we are within the cave bounds
                {
                    if (neighbourX != gridX || neighbourY != gridY) wallCount += cave[neighbourX, neighbourY]; //If the tile = 1 (wall) add it to the count
                }
                else wallCount++;
            }
        }

        return wallCount;
    }

    void DrawCave(Tilemap tilemap, TileBase hollow)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (cave[x, y] == 0) tilemap.SetTile(new Vector3Int(x, y, 0), hollow);
            }
        }
    }
}
