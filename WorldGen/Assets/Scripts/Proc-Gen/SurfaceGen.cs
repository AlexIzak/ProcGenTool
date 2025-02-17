using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SurfaceGen : MonoBehaviour
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
        public float heightValue, blendStrength;
    }
    
    private Vector3Int spawnPos;
    
    public WorldParams worldParams;

    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase grassTile, dirtTile, rockTile;

    // Generating map using noise algorithms -------------------------------------------------------------------------------------------------
    
    void GenerateTerrain()
    {

        //TODO - Create scriptable objects that have their own function to make biomes
        for (int x = 0; x < worldParams.width; ++x)
        {
            float p = Mathf.PerlinNoise(x / worldParams.smoothness, worldParams.seed);
            worldParams.depth = Mathf.RoundToInt(worldParams.heightValue * p);

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

                MixTiles(x, y);
            }
        }
    }

    void MixTiles(int x, int y)
    {
        // for (int x = 0; x < worldParams.width; ++x)
        // {
        // for (int y = 0; y < worldParams.depth; ++y)
        //  {

        //float switchChance = Mathf.PerlinNoise(x / 50f, 3000); - Checkerboard pattern
        float switchChance = Mathf.PerlinNoise(x/ worldParams.blendStrength, y / worldParams.blendStrength); //TODO - Get the resulting value to fluctuate for each tile - atm it fluctuates too rarely

        if (switchChance > 0.4f)
        {
            if (tilemap.GetTile(new Vector3Int(x, y, 0)) == dirtTile)
                tilemap.SetTile(new Vector3Int(x, y, 0), rockTile);
            else tilemap.SetTile(new Vector3Int(x, y, 0), dirtTile);
        }
        // }
        // }
    }

    private void Start()
    {
        worldParams.seed = UnityEngine.Random.Range(-100000, 100000);
        GenerateTerrain();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            worldParams.seed = UnityEngine.Random.Range(-100000, 100000);
            GenerateTerrain();
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
