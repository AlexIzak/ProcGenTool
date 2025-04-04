using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilesmeps : MonoBehaviour
{
    //This class stores and edits the tilemap
    [SerializeField]
    public Tilemap tileGrid;

    [SerializeField]
    private int width, height;

    //[SerializeField]
    int altitude;

    int maxWidth = 1600;
    int maxHeight = 900;

    private void Start()
    {
        if(width > maxWidth) 
            width = maxWidth;

        else if(height > maxHeight) 
            height = maxHeight;
    }

    //TODO Add a button that opens the editor window here

    /// <summary>
    /// Set a tile both visually and in the data grid (noiseGrid)
    /// </summary>
    /// <param name="x"></param> X position
    /// <param name="y"></param> Y position
    /// <param name="tile"></param> visual tile
    public void SetTile(int x, int y, MyTile tile)
    {
        if (!isOutofBounds(x, y))
        {
            tileGrid.SetTile(new Vector3Int(x, y, 0), tile);
        }
        else Debug.Log($"The tile at X : {x}, Y : {y} is out of bounds");
    }

    public MyTile GetTile(int x, int y)
    {
        //TODO Put it back to tile return type - (returns null when using an 'as' cast)
        return tileGrid.GetTile(new Vector3Int(x, y, 0)) as MyTile;
    }

    //Removes a tile
    public void ClearTile(int x, int y)
    {
        Destroy(tileGrid.GetTile(new Vector3Int(x, y, 0)));
        //dataGrid[x, y] = 0;

        //tileGrid.SetTile()
    }

    /// <summary>
    /// Returns true if tile is out of bounds
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool isOutofBounds(int x, int y)
    {
        //Check no tiles spawn outside map
        if(x < 0 || y < 0 || x > width || y >= height + altitude) return true;

        return false;
    }

    public int GetWidth() 
    {
        if(width > maxWidth)
            width = maxWidth;

        return width; 
    }
    public int GetHeight() 
    {
        if (height > maxHeight) 
            height = maxHeight;

        return height;
    }
    public int GetAltitude() { return altitude; }
    public int GetSurfaceMaxHeight()
    {
        List<int> altitudes = new List<int>();

        //Store all of the altitudes
        for (int x = 0; x < width; x++)
        {
            altitudes.Add(altitude);
        }

        int lastValue = 0;

        //Compare the altitudes till we get the biggest one
        foreach(int a in  altitudes)
        {
            if(lastValue < a) lastValue = a;
        }

        return lastValue;
    }

    public void SetAltitude(int input)
    {
        altitude = input;
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
