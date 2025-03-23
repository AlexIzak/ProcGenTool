using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OreGeneration", menuName = "Generation/Ores", order = 1)]
public class OresSO : BaseGeneration
{

    int[,] oreMap;

    [Range(0f, 0.9f)]
    public float decay = 0.8f;

    int visited = -1;
    int filled = 1;
    List<Vector2> queue;
    Vector2 coords;

    int width, height;

    public override void Generate(Tilesmeps world)
    {
        GenerateOre(world);
    }

    public void GenerateOre(Tilesmeps world)
    {
        this.width = world.width;
        this.height = world.height;
        oreMap = new int[width, height];

        int clumpCount = (width * height) / 100;

        for (int i = 0; i < clumpCount; i++)
        {
            int startX = UnityEngine.Random.Range(10, width - 10);
            int startY = UnityEngine.Random.Range(10, height - 10);

            LazyFloodFill(startX, startY);

            int oreType = UnityEngine.Random.Range(0, world.oreTiles.Count);

            //TODO Move this logic in the above for loop and have each clump be a random ore
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //If location is air - try 3 more times close by then stop
                    if (oreMap[x, y] == filled && world.dataGrid[x, y] != 0)
                    {
                        world.SetTile(x, y, world.oreTiles[oreType], 5);
                        oreMap[x, y] = visited;
                    }
                }
            }
        }

    }

    //Floodfill algorithm
    void LazyFloodFill(int x, int y)
    {
        float chance = 100f;

        float depth = (float)height / (float)y;

        decay = Normalize(depth);

        decay = Mathf.Clamp(decay, 0f, 0.9f);

        queue = new List<Vector2>();
        queue.Add(new Vector2(x, y));
        while (queue.Count != 0)
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
        if (x < 0 || x >= width || y < 0 || y >= height) { return false; }

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
}
