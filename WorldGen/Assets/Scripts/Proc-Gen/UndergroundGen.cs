using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UndergroundGen : MonoBehaviour
{
    Dictionary<int, TileBase> tileset;
    Dictionary<int, GameObject> tileGroups;
    //public Tilemap underground;
    public TileBase hollow;
    public TileBase dirt;
    public TileBase rock;

    int width = 160;
    int height = 90;

    [Range(4.0f, 20.0f)] //Recommended range
    float magnification = 7.0f;

    int xOffset = 0;
    int yOffset = 0;

    List<List<int>> noiseGrid = new List<List<int>>();
    List<List<TileBase>> tileGrid = new List<List<TileBase>>();

    // Start is called before the first frame update
    void Start()
    {
        CreateTileset();
        CreateTilemapGroups();
        GenerateUnderground();
    }

    private void CreateTileset()
    {
        tileset = new Dictionary<int, TileBase>();
        tileset.Add(0, hollow);
        tileset.Add(1, dirt);
        tileset.Add(2, rock);
        //Add more later
    }

    private void CreateTilemapGroups()
    {
        tileGroups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, TileBase> pair in tileset)
        {
            GameObject tilemap = new GameObject(pair.Value.name);
            tilemap.AddComponent<Tilemap>();
            tilemap.AddComponent<TilemapRenderer>();
            tilemap.transform.parent = gameObject.transform;
            tilemap.transform.localPosition = Vector3.zero;
            tileGroups.Add(pair.Key, tilemap);
        }
    }

    private void GenerateUnderground()
    {
        for (int x = 0; x < width; x++)
        {
            noiseGrid.Add(new List<int>());
            tileGrid.Add(new List<TileBase>());

            for (int y = 0; y < height; y++)
            {
                int tileID = GetIDwithPerlinNoise(x, y);
                noiseGrid[x].Add(tileID);
                CreateTile(tileID, x, y);
            }
        }
    }

    private void CreateTile(int tileID, int x, int y)
    {
        TileBase tile = tileset[tileID];
        GameObject tilemap = tileGroups[tileID];
        tilemap.GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), tile);
    }

    private int GetIDwithPerlinNoise(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise( (x - xOffset) / magnification,
            (y - yOffset) / magnification);

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);

        float scaledPerlin = clampPerlin * tileset.Count;
        if(scaledPerlin == tileset.Count)
            scaledPerlin -= 1; //Stops value from being out of range since index starts from 0

        return Mathf.FloorToInt(scaledPerlin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
