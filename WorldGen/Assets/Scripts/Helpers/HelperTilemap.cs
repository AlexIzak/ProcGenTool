using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HelperTilemap : MonoBehaviour
{
    public void isOutofBounds()
    {

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
