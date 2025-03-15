using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ore : MonoBehaviour
{
    int orePercentage = 5;
    float magnification = 4f;
    int[,] oreMap;

    public TileBase ore;

    [Range(0f, 0.9f)]
    public float decay = 0.8f;

    int visited = -1;
    int filled = 1;
    List<Vector2> queue;
    Vector2 coords;

    int width; 
    int height;

    //TODO Have different ore spawn in different stages

    //public void GenerateOre(int width, int height, Tilemap tilemap, int[,] terrainGrid)
    //{
    //    oreMap = new int[width, height];

    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height; y++)
    //        {
    //            //Use perlin noise to get a pattern
    //            oreMap[x, y] = GetIDwithPerlinNoise(x, y);

    //            //Have more ore in deeper areas
    //            int depthIncrement = height / (orePercentage);
    //            if (y < depthIncrement)
    //            {
    //                //Check if tile is valid - not an empty tile
    //                if (oreMap[x, y] >= 2 && terrainGrid[x, y] != 0) tilemap.SetTile(new Vector3Int(x, y, 0), ore);
    //            }
    //            else
    //            {
    //                int depthLayer = 1 + Mathf.FloorToInt(y / depthIncrement);

    //                if(depthLayer >= orePercentage) 
    //                    depthLayer = orePercentage - 1;

    //                if (oreMap[x, y] >= depthLayer && terrainGrid[x, y] != 0) tilemap.SetTile(new Vector3Int(x, y, 0), ore);
    //            }
    //        }
    //    }
    //}

    //private int GetIDwithPerlinNoise(int x, int y)
    //{
    //    float xOffset = Random.value;
    //    float yOffset = Random.value;

    //    float perlinX = ((float)x - xOffset) / magnification;
    //    float perlinY = ((float)y - yOffset) / magnification;

    //    float rawPerlin = Mathf.PerlinNoise(perlinX, perlinY);

    //    float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);

    //    float scaledPerlin = clampPerlin * orePercentage;
    //    //if (scaledPerlin == orePercentage)
    //    //    scaledPerlin -= 1; //Stops value from being out of range since index starts from 0

    //    return Mathf.FloorToInt(scaledPerlin);
    //}

    //Floodfill algorithm
    //TODO Try floodfill or branching algorithm for ores (pick random point - no noise required)
    //If location is air - try 3 more times close by then stop

    public void GenerateOre(int width, int height, Tilemap tilemap, int[,] terrainGrid)
    {
        this.width = width;
        this.height = height;
        oreMap = new int[width, height];

        int clumpCount = (width * height) / 100;

        for (int i = 0; i < clumpCount; i++)
        {
            int startX = UnityEngine.Random.Range(10, width - 10);
            int startY = UnityEngine.Random.Range(10, height - 10);

            LazyFloodFill(startX, startY);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (oreMap[x, y] == filled && terrainGrid[x, y] != 0) tilemap.SetTile(new Vector3Int(x, y, 0), ore);
            }
        }
    }

    void LazyFloodFill(int x, int y)
    {
        float chance = 100f;

        float depth = (float)height / (float)y;

        decay = Normalize(depth);

        decay = Mathf.Clamp(decay, 0f, 0.9f);

        queue = new List<Vector2>();
        queue.Add(new Vector2(x, y));
        while(queue.Count != 0)
        {
            coords = queue[0]; 
            queue.RemoveAt(0);
            oreMap[(int)coords.x, (int)coords.y] = filled;

            if (chance >= UnityEngine.Random.Range(1, 100))
            {
                HandleNeighbours();
                chance = chance * decay;
            }
        }
    }

    private float Normalize(float v)
    {
        return v - 1f / ((float)height / 10f) - 1f;
    }

    private void HandleNeighbours()
    {
        ValidateandAddtoQueue((int)coords.x, (int)coords.y - 1);

        ValidateandAddtoQueue((int)coords.x, (int)coords.y + 1);

        ValidateandAddtoQueue((int)coords.x - 1, (int)coords.y);

        ValidateandAddtoQueue((int)coords.x + 1, (int)coords.y);
    }

    private bool IsWithinBounds(float x, float y)
    {
        if(x < 0 || x >= width || y < 0 || y >= height) { return false; }

        return true;
    }

    private void ValidateandAddtoQueue(int x, int y)
    {
        if (IsWithinBounds(x, y) && oreMap[x, y] != filled && oreMap[x, y] != visited)
        {
            queue.Add(new Vector2(x, y));
            oreMap[x, y] = visited;
        }
    }

    //Branching algorithm



}
