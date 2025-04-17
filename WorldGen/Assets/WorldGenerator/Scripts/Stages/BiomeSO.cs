using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeGeneration", menuName = "Generation/Biome", order = 2)]
public class BiomeSO : BaseGeneration
{
    int[,] biomesMap;


    int visited = -1;
    int filled = 1;
    List<Vector2> queue;
    Vector2 coords;

    int width, height;

    [Header("The tiles used for the biomes (sand, granite etc)")]
    [SerializeField]
    List<MyTile> biomeTiles;

    [SerializeField]
    bool isSurfaceBiome;

    [Header("The tiles used for the surface of the biomes (snow, dry grass etc) \n Add in the same order as the above list of tiles to match biome with appropriate surface")]
    [SerializeField]
    List<MyTile> biomeSurfaceTiles;

    [Header("Surface Biome Attributes")]

    [Range(1, 5)]
    [SerializeField]
    int biomeCount = 2;

    [Header("Underground Biome Attributes")]

    [Range(0.9f, 0.99f)]
    [SerializeField]
    float decay = 0.95f;

    [Range(10, 50)]
    [SerializeField]
    int biomeSize = 30;

    [Range(1, 10)]
    [SerializeField]
    int biomeMultiplier = 5;

    public override void Generate(MyTilemap world)
    {
        if(isSurfaceBiome) 
            GenerateSurfaceBiomes(world);
        else 
            GenerateUndergroundBiomes(world);
    }

    private void GenerateSurfaceBiomes(MyTilemap world)
    {
        this.width = world.GetWidth();
        this.height = world.GetSurfaceMaxHeight() - world.GetHeight();

        int biomeSize = (width * height) / (50 * biomeCount);

        for (int i = 0; i < biomeCount; i++)
        {
            int startX = UnityEngine.Random.Range(0, width - biomeSize);
            int startY = world.GetHeight();

            int biomeType = UnityEngine.Random.Range(0, biomeTiles.Count);

            for (int x = startX; x < startX + biomeSize; x++)
            {
                for (int y = startY; y < startY + world.GetAltitude(); y++)
                {
                    if (world.GetTile(x, y) != null)
                    {
                        if(world.GetTile(x,y).Tags.Contains("Surface"))
                            world.SetTile(x, y, biomeSurfaceTiles[biomeType]);
                        else
                            world.SetTile(x, y, biomeTiles[biomeType]);
                    }
                }
            }
        }
    }

    private void GenerateUndergroundBiomes(MyTilemap world)
    {
        this.width = world.GetWidth();
        this.height = world.GetHeight();
        biomesMap = new int[width, height];

        int biomeCount = ((width * height) / (500 * biomeSize)) * biomeMultiplier;

        for (int i = 0; i < biomeCount; i++)
        {
            int startX = UnityEngine.Random.Range(30, width - 30);
            int startY = UnityEngine.Random.Range(50, height - 30);

            //TODO Change to the floodfill from oreSO
            LazyFloodFill(startX, startY);

            int biomeType = UnityEngine.Random.Range(0, biomeTiles.Count);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //Check for valid location
                    if (biomesMap[x, y] == filled && !world.GetTile(x,y).Tags.Contains("Hollow") && !world.GetTile(x, y).Tags.Contains("Ore"))
                    {
                        world.SetTile(x, y, biomeTiles[biomeType]);
                        biomesMap[x, y] = visited;
                    }
                }
            }
        }
    }

    //Floodfill algorithm
    void LazyFloodFill(int x, int y)
    {
        float chance = 100f;

        queue = new List<Vector2>();
        queue.Add(new Vector2(x, y));
        while (queue.Count != 0)
        {
            coords = queue[0];
            queue.RemoveAt(0);
            biomesMap[(int)coords.x, (int)coords.y] = filled;

            if (chance >= UnityEngine.Random.Range(1, 100))
            {
                HandleNeighbours();

                if (UnityEngine.Random.Range(0, biomeSize) == 0)
                    chance = chance * decay;
            }
        }
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
        if (x < 0 || x >= width || y < 0 || y >= height) { return false; }

        return true;
    }

    private void ValidateandAddtoQueue(int x, int y)
    {
        if (IsWithinBounds(x, y) && biomesMap[x, y] != filled && biomesMap[x, y] != visited)
        {
            queue.Add(new Vector2(x, y));
            biomesMap[x, y] = visited;
        }
    }
}
