using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


public class UndergroundGen : MonoBehaviour
{
    Dictionary<int, TileBase> tileset;
    Dictionary<int, GameObject> tileGroups;
    
    [Header("Based on how often they appear on the tilemap")]
    public TileBase average;
    public TileBase most;
    public TileBase least;

    [Header("Map dimensions")]
    [SerializeField] int width = 160;
    [SerializeField] int height = 90;

    [Range(4.0f, 20.0f)] //Recommended range
    [SerializeField] float magnification = 7.0f;

    [field:System.ComponentModel.ReadOnly(true)]    
    [SerializeField] int xOffset = 0;
    [SerializeField] int yOffset = 0;

    List<List<int>> noiseGrid = new List<List<int>>();
    List<List<TileHelper>> tileGrid = new List<List<TileHelper>>();

    [SerializeField]
    Tilemap map;

    //Background
    [SerializeField] SpriteRenderer bgWall;

    [Serializable]
    public struct SurfaceParams
    {
        //Size,
        public int width;

        [Unity.Collections.ReadOnly] public int depth;

        [field: Unity.Collections.ReadOnly]
        public float seed;

        [Range(0f, 30f)]
        public float smoothness;
    
        [Range(0f, 30f)]
        public float heightValue;
    }

    [SerializeField]
    private SurfaceParams surfaceParams;

    [SerializeField]
    CaveGen cave;

    // Start is called before the first frame update
    void Start()
    {

        float xPos = width / 2;
        float YPos = height / 2;

        Instantiate(bgWall, new Vector3(xPos, YPos, 1), Quaternion.identity);
        bgWall.drawMode = SpriteDrawMode.Tiled;
        bgWall.size = new Vector2(width, height);

        CreateTileset();

        Generate();
    }

    private void CreateTileset()
    {
        tileset = new Dictionary<int, TileBase>();
        tileset.Add(0, average);
        tileset.Add(1, most);
        tileset.Add(2, least);
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

    //TODO Parallax mapping for the background

    private void GenerateSurface()
    {
        surfaceParams.seed = UnityEngine.Random.Range(-100000, 100000);

        //TODO - Create scriptable objects that have their own function to make biomes
        for (int x = 0; x < surfaceParams.width; ++x)
        {
            float p = Mathf.PerlinNoise(x / surfaceParams.smoothness, surfaceParams.seed);
            surfaceParams.depth = Mathf.RoundToInt(surfaceParams.heightValue * p + 90f);

            //noise.pnoise
            //noise.cellular

            for (int y = 90; y < surfaceParams.depth; ++y)
            {
                //tileGroups[2].GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), tileset[2]);
                map.SetTile(new Vector3Int(x, y, 0), tileset[2]);
            }
        }
    }

    private void GenerateUnderground()
    {
        for (int x = 0; x < width; x++)
        {
            noiseGrid.Add(new List<int>());
            tileGrid.Add(new List<TileHelper>());

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
        TileHelper tile = ScriptableObject.CreateInstance<TileHelper>();
        tile.tileBase = tileset[tileID];
        //GameObject tilemap = tileGroups[tileID];
        //tilemap.GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), tile.tileBase);
        map.SetTile(new Vector3Int(x, y, 0), tile.tileBase);
        tile.SetPos(x, y);

        //TODO Set neighbouring tiles
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Generate();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            map.ClearAllTiles();
            //ClearMap();
        }
    }

    public void Generate()
    {
        xOffset = UnityEngine.Random.Range(-20, 20);
        yOffset = UnityEngine.Random.Range(-20, 20);

        //CreateTilemapGroups();
        GenerateUnderground();
        GenerateSurface();
        //GenerateCave();

        //Tilemap caveTilemap = tileGroups[1].GetComponent<Tilemap>();

        cave.GenerateCave(width / 4, height / 3, map, average);
    }

    public void SetData(int tileID) //Only swaps 1 tile because I can't access it in editor once I add 2 parameters
    {
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
        tileset.Remove(tilesetID);
        tileset.Add(tilesetID, average);
    }

    public void ClearMap()
    {
        foreach(var group in tileGroups)
        {
            group.Value.GetComponent<Tilemap>().ClearAllTiles();
            Destroy(group.Value);
        }

        //Destroy(bgWall.gameObject);
    }

    /** Ignore for now
    void GenerateCave()
    {
        //int radius = Mathf.FloorToInt((surfaceParams.width / 4) * Mathf.PerlinNoise(surfaceParams.width / 2f, surfaceParams.seed));
        int radius = 5;

        //TODO Generate random cave origin, but don't have them too close toghether
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
