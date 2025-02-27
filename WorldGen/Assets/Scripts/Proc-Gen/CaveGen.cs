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

    public void GenerateCave(int width, int height, Vector2 originPoint, Tilemap tilemap, TileBase hollow)
    {
        this.width = (int)originPoint.x + width;
        this.height = (int)originPoint.y + height;
        cave = new int[this.width, this.height];
        RandomFillCave(originPoint);

        for (int i = 0; i < 5; i++)
        {
            SmoothCave(originPoint);
            //float[] sorted = Array.Sort();
        }

        DrawCave(originPoint, tilemap, hollow);
    }

    private void RandomFillCave(Vector2 pos)
    {
        if (useRandomSeed)
        {
            seed = Mathf.FloorToInt(Time.time * 10f);
        }

        System.Random pseudoRandom = new System.Random(seed);

        for (int x = (int)pos.x; x < width; x++)
        {
            for (int y = (int)pos.y; y < height; y++)
            {
                if(x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    cave[x, y] = 1;
                }
                else cave[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    public void SmoothCave(Vector2 pos)
    {
        for (int x = (int)pos.x; x < width; x++)
        {
            for (int y = (int)pos.y; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y, pos);

                if (neighbourWallTiles > 4) cave[x, y] = 1;

                else if (neighbourWallTiles < 4) cave[x, y] = 0;
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY, Vector2 pos)
    {
        int wallCount = 0;

        //Check a 3 by 3 grid around a tile
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if(neighbourX >= pos.x && neighbourX < width &&  neighbourY >= pos.y && neighbourY < height) //Check we are within the cave bounds
                {
                    if (neighbourX != gridX || neighbourY != gridY) wallCount += cave[neighbourX, neighbourY]; //If the tile = 1 (wall) add it to the count
                }
                else wallCount++;
            }
        }

        return wallCount;
    }

    void DrawCave(Vector2 pos, Tilemap tilemap, TileBase hollow)
    {
        for (int x = (int)pos.x; x < width; x++)
        {
            for (int y = (int)pos.y; y < height; y++)
            {
                if (cave[x, y] == 0) tilemap.SetTile(new Vector3Int(x, y, 0), hollow);
            }
        }
    }
}
