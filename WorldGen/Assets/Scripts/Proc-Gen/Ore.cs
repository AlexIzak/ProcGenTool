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

    public void GenerateOre(int width, int height, Tilemap tilemap, int[,] terrainGrid)
    {
        oreMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Use perlin noise to get a pattern
                oreMap[x, y] = GetIDwithPerlinNoise(x, y);

                //Have more ore in deeper areas
                int depthIncrement = height / (orePercentage - 1);
                if (y < depthIncrement)
                {
                    //Check if tile is valid - not an empty tile
                    if (oreMap[x, y] > 2 && terrainGrid[x, y] != 0) tilemap.SetTile(new Vector3Int(x, y, 0), ore);
                }
                else
                {
                    int depthLayer = 2 + Mathf.FloorToInt(y / depthIncrement);

                    if(depthLayer >= orePercentage) 
                        depthLayer = orePercentage - 1;
          
                    if (oreMap[x, y] >= depthLayer && terrainGrid[x, y] != 0) tilemap.SetTile(new Vector3Int(x, y, 0), ore);
                }
            }
        }
    }

    private int GetIDwithPerlinNoise(int x, int y)
    {
        float xOffset = Random.value;
        float yOffset = Random.value;

        float perlinX = ((float)x - xOffset) / magnification;
        float perlinY = ((float)y - yOffset) / magnification;

        float rawPerlin = Mathf.PerlinNoise(perlinX, perlinY);

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);

        float scaledPerlin = clampPerlin * orePercentage;
        //if (scaledPerlin == orePercentage)
        //    scaledPerlin -= 1; //Stops value from being out of range since index starts from 0

        return Mathf.FloorToInt(scaledPerlin);
    }
}
