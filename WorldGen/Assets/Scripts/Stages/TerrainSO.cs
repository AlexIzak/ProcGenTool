using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainGeneration", menuName = "Generation/Terrain", order = -1)]
public class TerrainSO : BaseGeneration
{
    
    float xOffset = 0f;
    float yOffset = 0f;

    float magnification = 0f;

    int tileCount = 0;

    public override void Generate(Tilesmeps world)
    {

        xOffset = UnityEngine.Random.Range(-100f, 100f); //This value seems to get rid of the mirroring effect - mirroring happens when value is low
        yOffset = UnityEngine.Random.Range(-100f, 100f);

        magnification = UnityEngine.Random.Range(5f, 10f); //Recommended range

        GenerateUnderground(world);
        GenerateSurface(world);
    }

    //TODO Make the world generation use multiple tiles (e.g 5 dirt tiles, 5 rock tiles and 3 grass tiles)
    //TODO Use ruletile to place blocks in the apropriate orientation

    public void GenerateSurface(Tilesmeps world)
    {
        world.surfaceParams.seed = UnityEngine.Random.Range(-100000, 100000);

        for (int x = 0; x < world.width; ++x)
        {
            //Calculate the height of the surface with perlin noise
            float p = Mathf.PerlinNoise(x / world.surfaceParams.smoothness, world.surfaceParams.seed);
            world.surfaceParams.altitude = Mathf.RoundToInt(world.surfaceParams.heightValue * p + world.height);

            for (int y = world.height; y < world.surfaceParams.altitude; ++y)
            {
                int grassType = UnityEngine.Random.Range(0, world.surfaceTiles.Count);

                int dirtType = UnityEngine.Random.Range(0, world.dirtTiles.Count);

                //world.SetTile(x, y, world.basicTiles[2], 1);// 2 is dirt

                world.SetTile(x, y, world.dirtTiles[dirtType], 1);

                //Set the top tiles to grass
                if (y + 1 >= world.surfaceParams.altitude)
                    world.SetTile(x, y, world.surfaceTiles[grassType], 3);
            }
        }
    }
    
    public void GenerateUnderground(Tilesmeps world)
    {
        tileCount = world.basicTiles.Count;

        for (int x = 0; x < world.width; x++)
        {
            for (int y = 0; y < world.height; y++)
            {
                int tileID = GetIDwithPerlinNoise(x, y);
                world.dataGrid[x, y] = tileID;
                world.SetTile(x, y, world.basicTiles[tileID], tileID);
            }
        }
    }

    private int GetIDwithPerlinNoise(int x, int y)
    {
        float perlinX = (x - xOffset) / magnification;
        float perlinY = (y - yOffset) / magnification;

        float rawPerlin = Mathf.PerlinNoise(perlinX, perlinY);

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);

        float scaledPerlin = clampPerlin * tileCount;
        if (scaledPerlin == tileCount)
            scaledPerlin -= 1; //Stops value from being out of range since index starts from 0

        return Mathf.FloorToInt(scaledPerlin);
    }
}
