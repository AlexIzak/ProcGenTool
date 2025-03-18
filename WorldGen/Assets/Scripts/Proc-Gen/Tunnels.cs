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

    TileBase hollow;

    //int boundary = 10;

    //public void GenerateTunnels(int width, int height, Tilemap tilemap, TileBase hollow)
    //{
    //    this.width = width;
    //    this.height = height;
    //    map = new float[width, height];

    //    DrawTunnel(tilemap, hollow);
    //}

    public void GenerateTunnels(Tilesmeps world)
    {
        this.width = world.width;
        this.height = world.height;
        map = new float[width, height];

        DrawTunnel(world);
    }

    //private void DrawTunnel(Tilemap tilemap, TileBase hollow)
    //{
    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height; y++)
    //        {

    //            float n = noise.cellular(new float2(x / 10f, y / 10f)).x;
    //            map[x, y] = n;

    //            //Change the two values below to create more interesting tunnel shapes
    //            if (map[x, y] > 0.6f && map[x, y] < 0.8f) tilemap.SetTile(new Vector3Int(x, y, 0), hollow); //Will do for now
    //        }
    //    }
    //}

    private void DrawTunnel(Tilesmeps world)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                float n = noise.cellular(new float2(x / 10f, y / 10f)).x;
                map[x, y] = n;

                //Change the two values below to create more interesting tunnel shapes
                //if (map[x, y] > 0.6f && map[x, y] < 0.8f) world.ClearTile(x, y);
                if (map[x, y] > 0.6f && map[x, y] < 0.8f) world.SetTile(x, y, hollow, 0);
            }
        }
    }

    public float[,] GetTunnelStructure() { return map; }

}
