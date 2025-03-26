using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : TileBase
{
    //TODO Add tags to all tiles (List<string>)
    //E.G Terrain / gravity / ore / iluminated
    public List<string> Tags {  get { return tags; } }

    [SerializeField]
    private List<string> tags;
}
