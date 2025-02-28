using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Tunnels : MonoBehaviour
{
    int width, height;

    int[,] map;
    Vector2[] points;
    int tunnelFrequency;
    int boundary = 10;

    //Testing
    //public Tilemap grid;
    //public TileBase ground;
    //public TileBase empty;
    //public TileBase rock;

    //private void Start()
    //{
    //    grid = GetComponent<Tilemap>();

    //    GenerateTunnels(160, 90, 100, grid, empty);
    //}

    public void GenerateTunnels(int width, int height, int frequency, Tilemap tilemap, TileBase hollow)
    {
        this.width = width;
        this.height = height;
        map = new int[width, height];
        
        tunnelFrequency = frequency;

        DrawTunnel(tilemap, hollow);
    }

    void Setup()
    {
        int pointSample = (width * height) / tunnelFrequency;

        points = new Vector2[pointSample];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector2(UnityEngine.Random.Range(boundary, width - boundary), UnityEngine.Random.Range(boundary, height - boundary));
        }
    }

    private void DrawTunnel(Tilemap tilemap, TileBase hollow)
    {
        Setup();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //tilemap.SetTile(new Vector3Int(x, y, 0), ground);


                //float scalar = 1;
                //float n = noise.cellular2x2(new Vector2(x*scalar, y*scalar)).x;
                ////float n = noise.cnoise(new float2(x/width, y/height));
                //if (n < 0.5f)
                //{
                //    tilemap.SetTile(new Vector3Int(x, y), hollow);
                //}




                float[] distances = new float[points.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    //tilemap.SetTile(new Vector3Int((int)points[i].x, (int)points[i].y, 0), rock);


                    //float n = noise.cnoise(new float2(points[i].x, points[i].y));

                    float d = Vector2.Distance(new Vector2(x, y), points[i]);

                    //float dx = (x - points[i].x) * (x - points[i].x);
                    //float dy = (y - points[i].y) * (y - points[i].y);
                    //float dist = Mathf.Sqrt(dx + dy);
                    distances[i] = d;
                    //int value = GetIDwithWorleyNoise(n);
                    //map[x,y] = value; 
                }

                int n = 0; //Decides which closest point the algorithm considers (0 being the first)
                Array.Sort(distances);

                float noise = distances[n];

                //int index = x + y * width; //working with a 1D array
                //TODO Get the distance to be used properly for the mapping of the grid
                int value = GetIDwithWorleyNoise(noise);
                map[x,y] = value;

                //TODO Change the two values below to create more interesting tunnel shapes
                if (map[x,y] > 15 && map[x, y] < 20) tilemap.SetTile(new Vector3Int(x, y, 0), hollow);
            }
        }
    }

    private int GetIDwithWorleyNoise(float noise)
    {
        //float rawNoise = noise / 10f;

        //float clampNoise = Mathf.Clamp(noise, 0.0f, 1.0f);

        //float scaledNoise = clampNoise;
        //if (scaledNoise == 2f)
            //scaledNoise -= 0.1f; //Stops value from being out of range (above 1 after rounding down)

        //return Mathf.FloorToInt(scaledNoise);
        return Mathf.FloorToInt(noise);
    }
}
