using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{
    [Serializable]
    public struct WorldParams
    {
        //Size,
        public int width;
        public int depth;

        //depth of map (surface - 0 to 20, underground - 20 to 80, cavern - 80+)
        public int surfaceStart;
        public int surfaceLimit;
        public int cavernLimit;

        public float seed;

        [Range(0f, 500f)]
        public float smoothness;
        [Range(0f, 500f)]
        public int undergroundLimit;
        [Range(0f, 500f)]
        public float heightValue;
    }
    
    private Vector3Int spawnPos;
    
    public WorldParams worldParams;

    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase grassTile, dirtTile, rockTile;

    public int[] GenerateTerrain()
    {
        int arraySize = worldParams.width * worldParams.depth;
        int[] indices = new int[arraySize];
        
        int index = 0;

        for (int x = 0; x < worldParams.width; ++x)
        {
            for (int y = 0; y < worldParams.depth; ++y)
            {
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                int tileType;

                if (x == 0) tileType = 1;
                else if(x < 20) tileType = 2;
                else tileType = 3;

                //Store each value (decides the tile) in an array, index decides position
                indices.SetValue(tileType, index);
                ++index;

                //Option 1 - only stores location so I would have to figure out tile allocation after - simpler solution found
                //Vector2[] world = { };
                //world.SetValue(new Vector2(spawnPos.x, spawnPos.y), index);
                //index++;

                //Option 2 - 2D array of values (used to decide tile type), the location can be derived from the indices of each value - Overkill
                //int[,] world2 = { };
                //world2.SetValue(1, indices);
                Debug.Log(new Vector2(x, y));
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
        }

        return indices;
    }

    // Generating map using noise algorithms -------------------------------------------------------------------------------------------------
    
    void Generate()
    {

        //TODO - Create scriptable objects that have their own function to make biomes
        for (int x = 0; x < worldParams.width; ++x)
        {
            worldParams.depth = Mathf.RoundToInt(worldParams.heightValue * Mathf.PerlinNoise(x / worldParams.smoothness, worldParams.seed));

            //noise.pnoise
            //noise.cellular

            int minStonePos = worldParams.depth - worldParams.cavernLimit;
            int maxStonePos = worldParams.depth - worldParams.undergroundLimit;
            int totalStone = Mathf.RoundToInt(worldParams.undergroundLimit * Mathf.PerlinNoise(x / worldParams.smoothness, worldParams.seed));

            for (int y = 0; y < worldParams.depth; ++y)
            {
                if(y < totalStone) tilemap.SetTile(new Vector3Int(x, y, 0), rockTile);
                else tilemap.SetTile(new Vector3Int(x, y, 0), dirtTile);

                if (totalStone == worldParams.depth) tilemap.SetTile(new Vector3Int(x, y, 0), rockTile);
                //else tilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
            }
        }
    }

    private void Start()
    {
        worldParams.seed = UnityEngine.Random.Range(-100000, 100000);
        Generate();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            worldParams.seed = UnityEngine.Random.Range(-100000, 100000);
            Generate();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            tilemap.ClearAllTiles();
        }
    }

    // Generating map using noise algorithms -------------------------------------------------------------------------------------------------

    void GenerateCaves()
    {
        //Algorithm goes here

    }

    void GenerateOre()
    {
        //Algorithm goes here

    }
}
