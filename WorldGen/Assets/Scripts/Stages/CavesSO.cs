using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CaveGeneration", menuName = "Generation/Caves", order = 0)]
public class CavesSO : BaseGeneration
{

    int width, height;

    int seed;
    bool useRandomSeed = true;

    int randomFillPercent;

    int[,] cave;
    float[,] tunnels;

    //Public parameters
    [Range(1, 5)]
    [SerializeField]
    int caveSize = 3;

    [Range(2, 5)]
    [SerializeField]
    int caveMultiplier = 2;

    [Header("The bigger the value the emptier the cave")]
    [Range(2, 8)]
    [SerializeField]
    int caveSmoothness = 5;

    [SerializeField]
    MyTile hollow;

    public override void Generate(Tilesmeps world)
    {

        //Cave Parameters
        int caveWidth = (world.GetWidth() / 20) * caveSize; //Decent size values
        int caveHeight = (world.GetHeight() / 15) * caveSize;

        //Get more or less caves depending on map size and cave size
        int caveCount = (world.GetWidth() + world.GetHeight()) / (caveWidth + caveHeight) * caveMultiplier;

        for (int i = 0; i < caveCount; i++)
        {
            //Getting a start position for the cave
            int xPos = UnityEngine.Random.Range(10, world.GetWidth() - caveWidth);
            int yPos = UnityEngine.Random.Range(10, world.GetHeight() - caveHeight);
            Vector2 caveOrigin = new Vector2(xPos, yPos);

            if(caveSize > 1)
            {
                //Making each cave increasingly smaller
                caveWidth -= i;
                caveHeight -= i;
            }

            GenerateCave(caveWidth, caveHeight, caveOrigin, world);
        }

        GenerateTunnels(world);
    }

    //Cave Generation Logic
    public void GenerateCave(int width, int height, Vector2 originPoint, Tilesmeps world)
    {
        this.width = (int)originPoint.x + width;
        this.height = (int)originPoint.y + height;
        cave = new int[this.width, this.height];
        RandomFillCave(originPoint);

        for (int i = 0; i < caveSmoothness; i++)
        {
            SmoothCave(originPoint);
        }

        DrawCave(originPoint, world);
    }

    private void RandomFillCave(Vector2 pos)
    {
        randomFillPercent = UnityEngine.Random.Range(40, 50);

        if (useRandomSeed)
        {
            seed = Mathf.FloorToInt(Time.time * 10f);
        }

        System.Random pseudoRandom = new System.Random(seed);

        for (int x = (int)pos.x; x < width; x++)
        {
            for (int y = (int)pos.y; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    cave[x, y] = 1; //Wall
                }
                else cave[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
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

                if (neighbourWallTiles > 4) cave[x, y] = 1; //Wall

                else if (neighbourWallTiles < 4) cave[x, y] = 0; //Empty
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
                if (neighbourX >= pos.x && neighbourX < width && neighbourY >= pos.y && neighbourY < height) //Check we are within the cave bounds
                {
                    if (neighbourX != gridX || neighbourY != gridY) wallCount += cave[neighbourX, neighbourY]; //If the tile = 1 (wall) add it to the count
                }
                else wallCount++;
            }
        }

        return wallCount;
    }

    void DrawCave(Vector2 pos, Tilesmeps world)
    {
        for (int x = (int)pos.x; x < width; x++)
        {
            for (int y = (int)pos.y; y < height; y++)
            {
                //if (cave[x, y] == 0) world.ClearTile(x, y);
                //if (cave[x, y] == 0) world.SetTile(x, y, null);
                if (cave[x, y] == 0) world.SetTile(x, y, hollow);
            }
        }
    }

    //Tunnels Generation Logic
    public void GenerateTunnels(Tilesmeps world)
    {
        tunnels = new float[world.GetWidth(), world.GetHeight()];

        DrawTunnel(world);
    }

    private void DrawTunnel(Tilesmeps world)
    {
        float offset = UnityEngine.Random.Range(8f, 16f);

        for (int x = 0; x < world.GetWidth(); x++)
        {
            for (int y = 0; y < world.GetHeight(); y++)
            {
                float n = noise.cellular(new float2(x / offset, y / offset)).x;
                tunnels[x, y] = n;

                //Change the two values below to create more interesting tunnel shapes
                //if (tunnels[x, y] > 0.6f && tunnels[x, y] < 0.8f) world.SetTile(x, y, null);
                if (tunnels[x, y] > 0.6f && tunnels[x, y] < 0.8f) world.SetTile(x, y, hollow);
            }
        }
    }
}
