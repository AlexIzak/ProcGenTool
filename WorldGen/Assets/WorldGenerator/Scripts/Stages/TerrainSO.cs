using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TerrainGeneration", menuName = "Generation/Terrain", order = -1)]
public class TerrainSO : BaseGeneration
{
    [Header("The tiles used for the generic terrain (dirt, rock etc.)")]
    [SerializeField]
    List<MyTile> basicTiles;

    [Header("Different variations of dirt tiles for the surface")]
    [SerializeField]
    List<MyTile> dirtTiles;

    [Header("The tiles used for the surface top (grass, snow etc)")]
    [SerializeField] 
    List<MyTile> surfaceTiles;

    [Header("Amount of detail in the underground")]
    [SerializeField]
    [Range(5f, 10f)]
    float magnification = 5f;

    [Range(0f, 30f)]
    [SerializeField]
    float smoothness;

    [Range(0f, 30f)]
    [SerializeField]
    float heightValue;

    //[Serializable]
    //struct SurfaceParams
    //{
    //    //Size,
    //    //public int width;

    //    public int altitude;

    //    public int seed;

    //    [Range(0f, 30f)]
    //    public float smoothness;

    //    [Range(0f, 30f)]
    //    public float heightValue;
    //}

    //[SerializeField]
    //SurfaceParams surfaceParams;

    float xOffset = 0f;
    float yOffset = 0f;

    bool useRandomSeed = true;

    int seed;

    public override void Generate(MyTilemap world)
    {

        xOffset = UnityEngine.Random.Range(-100f, world.GetWidth()/2);
        yOffset = UnityEngine.Random.Range(-100f, world.GetHeight()/2);

        //magnification = UnityEngine.Random.Range(5f, 10f); //Recommended range

        GenerateUnderground(world);
        GenerateSurface(world);
    }

    //TODO Make the world generation use multiple tiles (e.g 5 dirt tiles, 5 rock tiles and 3 grass tiles)
    //TODO Use ruletile to place blocks in the apropriate orientation

    public void GenerateSurface(MyTilemap world)
    {
        if (useRandomSeed)
        {
            seed = Mathf.FloorToInt(Time.time * 100f);
        }

        System.Random pseudoRandom = new System.Random(seed);

        for (int x = 0; x < world.GetWidth(); ++x)
        {
            //Calculate the height of the surface with perlin noise
            float p = Mathf.PerlinNoise(x / smoothness, seed);
            int alt = Mathf.RoundToInt(heightValue * p + world.GetHeight());
            world.SetAltitude(alt);

            for (int y = world.GetHeight(); y < world.GetAltitude(); ++y)
            {
                int grassType = UnityEngine.Random.Range(0, surfaceTiles.Count);

                int dirtType = UnityEngine.Random.Range(0, dirtTiles.Count);

                world.SetTile(x, y, dirtTiles[dirtType]);
                //world.SetTile(x, y, dirt, 1);

                //Set the top tiles to grass
                if (y + 1 >= world.GetAltitude())
                    world.SetTile(x, y, surfaceTiles[grassType]);
                    //world.SetTile(x, y, grass, 3);

                //Debug.Log("Placed!");
            }
        }
    }
    
    public void GenerateUnderground(MyTilemap world)
    {

        for (int x = 0; x < world.GetWidth(); x++)
        {
            for (int y = 0; y < world.GetHeight(); y++)
            {
                int tileID = GetIDwithPerlinNoise(x, y);
            
                world.SetTile(x, y, basicTiles[tileID]);
            }
        }
    }

    private int GetIDwithPerlinNoise(int x, int y)
    {
        float perlinX = (x - xOffset) / magnification;
        float perlinY = (y - yOffset) / magnification;

        float rawPerlin = Mathf.PerlinNoise(perlinX, perlinY);

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);

        float scaledPerlin = clampPerlin * basicTiles.Count;
        if (scaledPerlin == basicTiles.Count)
            scaledPerlin -= 1; //Stops value from being out of range since index starts from 0

        return Mathf.FloorToInt(scaledPerlin);
    }
}
