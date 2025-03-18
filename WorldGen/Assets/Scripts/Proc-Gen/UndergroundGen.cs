using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;


public class UndergroundGen : MonoBehaviour
{
    Dictionary<int, TileBase> tileset;
    //Dictionary<int, GameObject> tileGroups;
    
    [Header("Based on how often they appear on the tilemap")]
    public TileBase average;
    public TileBase most;
    public TileBase least;
    public TileBase grass;

    [Range(4.0f, 20.0f)] //Recommended range
    [SerializeField] float magnification = 7.0f;

    [field:System.ComponentModel.ReadOnly(true)]    
    [SerializeField] float xOffset = 0;
    [SerializeField] float yOffset = 0;

    //Background
    [SerializeField] SpriteRenderer bgWall;

    [SerializeField]
    Ore ore;

    // Start is called before the first frame update
    //void Start()
    //{
    //    float xPos = width / 2;
    //    float YPos = height / 2;

    //    Instantiate(bgWall, new Vector3(xPos, YPos, 1), Quaternion.identity);
    //    bgWall.drawMode = SpriteDrawMode.Tiled;
    //    bgWall.size = new Vector2(width, height);

    //    CreateTileset();

    //    Generate();
    //}

    //TODO Change this to either an array or list
    public void CreateTileset()
    {
        tileset = new Dictionary<int, TileBase>();
        tileset.Add(0, average);
        tileset.Add(1, most);
        tileset.Add(2, least);
        //Add more later
    }

    //TODO Parallax mapping for the background

    public void GenerateSurface(Tilesmeps world)
    {
        world.surfaceParams.seed = UnityEngine.Random.Range(-100000, 100000);

        //TODO - Create scriptable objects that have their own function to make biomes
        for (int x = 0; x < world.width; ++x)
        {
            float p = Mathf.PerlinNoise(x / world.surfaceParams.smoothness, world.surfaceParams.seed);
            world.surfaceParams.altitude = Mathf.RoundToInt(world.surfaceParams.heightValue * p + world.height);

            for (int y = world.height; y < world.surfaceParams.altitude; ++y)
            {
                world.SetTile(x, y, tileset[2], 1);

                if (y + 1 >= world.surfaceParams.altitude) 
                    world.SetTile(x, y, grass, 3);
            }
        }
    }

    public void GenerateUnderground(Tilesmeps world)
    {
        //noiseGrid = new int[width, height];

        for (int x = 0; x < world.width; x++)
        {
            for (int y = 0; y < world.height; y++)
            {
                int tileID = GetIDwithPerlinNoise(x, y);
                world.dataGrid[x, y] = tileID;
                world.SetTile(x, y, tileset[tileID], tileID);
            }
        }
    }

    private int GetIDwithPerlinNoise(int x, int y)
    {
        float perlinX = (x - xOffset) / magnification;
        float perlinY = (y - yOffset) / magnification;

        float rawPerlin = Mathf.PerlinNoise(perlinX, perlinY);

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);

        float scaledPerlin = clampPerlin * tileset.Count;
        if(scaledPerlin == tileset.Count)
            scaledPerlin -= 1; //Stops value from being out of range since index starts from 0

        return Mathf.FloorToInt(scaledPerlin);
    }

    //public void Generate()
    //{
    //    xOffset = UnityEngine.Random.Range(-20f, 20f);
    //    yOffset = UnityEngine.Random.Range(-20f, 20f);

    //    CreateTileset();

    //    GenerateUnderground();
    //    GenerateSurface();

    //    //Cave Gen
    //    int caveWidth = width / 8; //Decent size values
    //    int caveHeight = height / 6;

    //    //Get more or less caves depending on map size and cave size
    //    int caveCount = (width + height) / (caveWidth + caveHeight);

    //    for (int i = 0; i < caveCount; i++)
    //    {
    //        //Getting a start position for the cave
    //        int xPos = UnityEngine.Random.Range(10, width - caveWidth);
    //        int yPos = UnityEngine.Random.Range(10, height - caveHeight);
    //        Vector2 caveOrigin = new Vector2(xPos, yPos);

    //        //Making each cave increasingly smaller
    //        caveWidth -= i;
    //        caveHeight -= i;

    //        cave.GenerateCave(caveWidth, caveHeight, caveOrigin, map, average);

    //        UpdateNoiseGrid(xPos, yPos, caveWidth, caveHeight);
    //    }

    //    //Tunnels Gen
    //    tunnels.GenerateTunnels(width, height, map, average);
    //    //TODO Test - map does not draw
    //    UpdateNoiseGrid(0, 0, width, height);

    //    //Ore Gen
    //    ore.GenerateOre(width, height, map, noiseGrid);
    //}

    public void SetData(int tileID) //Only swaps 1 tile because I can't access it in editor once I add 2 parameters
    {
        //TODO Add wrapper function
        tileset.Remove(0);

        switch (tileID)
        {
            case 0:
                tileset.Add(0, average);
                break;

            case 1:
                tileset.Add(0, most);
                break;

            case 2:
                tileset.Add(0, least);
                break;

            default:
                tileset.Add(0, average);
                break;
        }

        Debug.Log(tileID);
    }

    public void SwapTile(int tilesetID) //For the UI that changes the tiles - doesnt work 
    {
        //tileset.Remove(tilesetID);
        tileset.Add(tilesetID, average);
    }


    /** Ignore for now
    void GenerateCave()
    {
        //int radius = Mathf.FloorToInt((surfaceParams.width / 4) * Mathf.PerlinNoise(surfaceParams.width / 2f, surfaceParams.seed));
        int radius = 5;

        // Generate random cave origin, but don't have them too close toghether
        int randX = UnityEngine.Random.Range(20, width - 20);
        int randY = UnityEngine.Random.Range(20, height - 20);
        Vector2 center = new Vector2(randX, randY);

        for (int x = -radius; x < radius; x++)
        {
            //Improve noise generation
            radius = Mathf.FloorToInt((surfaceParams.width / 5) * Mathf.PerlinNoise((x - xOffset) / magnification, surfaceParams.seed));
            for (int y = -radius; y < radius; y++)
            {
                if(x*x + y*y <= radius*radius)
                {
                    tileGroups[1].GetComponent<Tilemap>().SetTile(new Vector3Int((int)center.x + x, (int)center.y + y, 0), tileset[0]);
                    tileGroups[2].GetComponent<Tilemap>().SetTile(new Vector3Int((int)center.x + x, (int)center.y + y, 0), tileset[0]);
                }
            }
        }
    }**/
}
