using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "Tile", order = 0)]
public class MyTile : Tile
{
    //TODO Add tags to all tiles (List<string>)
    //E.G Terrain / gravity / ore / iluminated
    public List<string> Tags {  get { return tags; } }

    [SerializeField]
    private List<string> tags;
}
