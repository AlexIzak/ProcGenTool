using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Tunnels : MonoBehaviour
{
    int width, height;

    float[,] map;
    int boundary = 10;

    //Testing
    //public Tilemap grid;
    //public TileBase ground;
    //public TileBase empty;
    //public TileBase rock;

    //private void Start()
    //{
    //    grid = GetComponent<Tilemap>();

    //    GenerateTunnels(160, 90, 200, grid, empty);
    //}

    public void GenerateTunnels(int width, int height, Tilemap tilemap, TileBase hollow)
    {
        this.width = width;
        this.height = height;
        map = new float[width, height];

        //for (int i = 0; i < 5; i++)
        //{
        //    SmoothCave();
        //}

        DrawTunnel(tilemap, hollow);
    }

    private void DrawTunnel(Tilemap tilemap, TileBase hollow)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //tilemap.SetTile(new Vector3Int(x, y, 0), ground);

                float n = noise.cellular(new float2(x / 10f, y / 10f)).x;
                map[x, y] = n;

                //Change the two values below to create more interesting tunnel shapes
                if (map[x, y] > 0.6f && map[x, y] < 0.8f) tilemap.SetTile(new Vector3Int(x, y, 0), hollow); //Will do for now
            }
        }
    }

    public float[,] GetTunnelStructure() { return map; }

    public void SmoothCave()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4) map[x, y] = 1f;

                else if (neighbourWallTiles < 4) map[x, y] = 0.6f;
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

        //Check a 3 by 3 grid around a tile
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) //Check we are within the cave bounds
                {
                    if (neighbourX != gridX || neighbourY != gridY) wallCount += Mathf.FloorToInt(map[neighbourX, neighbourY]); //If the tile = 1 (wall) add it to the count
                }
                else wallCount++;
            }
        }

        return wallCount;
    }
}
