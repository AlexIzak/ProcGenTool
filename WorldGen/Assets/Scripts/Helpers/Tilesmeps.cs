using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class Tilesmeps : MonoBehaviour
{
    //This class stores and edits the tilemap
    [SerializeField]
    public Tilemap tileGrid;

    public int width, height;

    [Serializable]
    public struct SurfaceParams
    {
        //Size,
        //public int width;

        [Unity.Collections.ReadOnly] public int altitude;

        [field: Unity.Collections.ReadOnly]
        public float seed;

        [Range(0f, 30f)]
        public float smoothness;

        [Range(0f, 30f)]
        public float heightValue;
    }

    [SerializeField]
    public SurfaceParams surfaceParams;

    [HideInInspector]
    public int[,] dataGrid;

    //[Header("Different variations of rock tiles")]
    ////[SerializeField]
    //public List<TileBase> rockTiles;


    private void Awake()
    {
        dataGrid = new int[width, height];
    }

    /// <summary>
    /// Set a tile both visually and in the data grid (noiseGrid)
    /// </summary>
    /// <param name="x"></param> X position
    /// <param name="y"></param> Y position
    /// <param name="tile"></param> visual tile
    /// <param name="tileID"></param> The value stored in the array for this tile speciffically
    public void SetTile(int x, int y, TileBase tile, int tileID)
    {

        if (!isOutofBounds(x, y))
        {
            tileGrid.SetTile(new Vector3Int(x, y, 0), tile);
            //dataGrid[x, y] = tileID;
        }
        //else Debug.Log("Out of bounds");
    }

    public TileBase GetTile(int x, int y)
    {
        //TODO Put it back to tile return type - (returns null when using an 'as' cast)
        return tileGrid.GetTile(new Vector3Int(x, y, 0));
    }

    //Removes a tile
    public void ClearTile(int x, int y)
    {
        Destroy(tileGrid.GetTile(new Vector3Int(x, y, 0)));
        dataGrid[x, y] = 0;

        //tileGrid.SetTile()
    }

    //Has stages as SOs stored inside another public SO that the user would see

    public bool isOutofBounds(int x, int y)
    {
        //Check no tiles spawn outside map
        if(x < 0 || y < 0 || x > width || y >= height) return true;

        return false;
    }

    /// <summary>
    /// Function that dynamically swaps the backround image according to the depth of the tile location (use mouse location later)
    /// </summary>
    /// <param name="tilemap"></param> The tilemap holding the tiles
    /// <param name="layers"></param> The amount of changes required based on ground layer
    /// <param name="swapInterval"></param> The amount of tiles between each layer/change
    public void ChangeBackground(Tilemap tilemap, int layers, int swapInterval)
    {
        //When below a certain depth, swap backround (e.g. surface to underground to cavern)
        //TODO Implement Dan's suggestion of having 4 images and moving them around based on camera pos
    }
}
