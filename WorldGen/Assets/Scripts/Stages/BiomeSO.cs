using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BiomeGeneration", menuName = "Generation/Biome", order = 2)]
public class BiomeSO : BaseGeneration
{
    int[,] biome;

    [Range(0f, 0.9f)]
    public float decay;

    int visited = -1;
    int filled = 1;
    List<Vector2> queue;
    Vector2 coords;

    int width, height;

    [Header("The tiles used for the biomes (sand, granite etc)")]
    //[SerializeField]
    public List<TileBase> biomeTiles;

    public override void Generate(Tilesmeps world)
    {
        GenerateBiome(world);
    }

    private void GenerateBiome(Tilesmeps world)
    {
        this.width = world.width;
        this.height = world.height;
        biome = new int[width, height];

        int biomeCount = (width * height) / 10000;

        for (int i = 0; i < biomeCount; i++)
        {
            int startX = UnityEngine.Random.Range(30, width - 30);
            int startY = UnityEngine.Random.Range(50, height - 30);

            LazyFloodFill(startX, startY);

            int biomeType = UnityEngine.Random.Range(0, biomeTiles.Count);

            //TODO Move this logic in the above for loop and have each clump be a random ore
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //If location is air - try 3 more times close by then stop
                    if (biome[x, y] == filled && world.dataGrid[x, y] != 0 && world.dataGrid[x, y] != 5)
                    {
                        world.SetTile(x, y, biomeTiles[biomeType], 10);
                        biome[x, y] = visited;
                    }
                }
            }
        }
    }

    //Floodfill algorithm
    void LazyFloodFill(int x, int y)
    {
        float chance = 100f;

        //float depth = (float)height / (float)y;

        //decay = Normalize(depth);

        decay = 0.95f;

        //decay = Mathf.Clamp(decay, 0f, 0.9f);

        queue = new List<Vector2>();
        queue.Add(new Vector2(x, y));
        while (queue.Count != 0)
        {
            coords = queue[0];
            queue.RemoveAt(0);
            biome[(int)coords.x, (int)coords.y] = filled;

            if (chance >= UnityEngine.Random.Range(1, 100))
            {
                HandleNeighbours();
                chance = chance * decay;
            }
        }
    }

    //private float Normalize(float v)
    //{
    //    return v - 1f / ((float)height / 10f) - 1f;
    //}

    private void HandleNeighbours()
    {
        ValidateandAddtoQueue((int)coords.x, (int)coords.y - 1);
        ValidateandAddtoQueue((int)coords.x, (int)coords.y + 1);
        ValidateandAddtoQueue((int)coords.x - 1, (int)coords.y);

        ValidateandAddtoQueue((int)coords.x + 1, (int)coords.y);
    }

    private bool IsWithinBounds(float x, float y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) { return false; }

        return true;
    }

    private void ValidateandAddtoQueue(int x, int y)
    {
        if (IsWithinBounds(x, y) && biome[x, y] != filled && biome[x, y] != visited)
        {
            queue.Add(new Vector2(x, y));
            biome[x, y] = visited;
        }
    }
}
