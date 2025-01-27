 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    public TileBase groundTile;
    public Tilemap tilemap;

    public Vector3Int spawnPos;

    public int screenWidth;
    public int screenHeight;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < screenWidth; i++)
        {
            for (int j = 0; j < screenHeight; j++)
            {
                spawnPos.x = i - screenWidth/2;
                spawnPos.y = j - screenHeight;
                Paint();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Paint")]
    void Paint()
    {
        //TODO Get different tiles to appear at different percentages based on depth(y)
        tilemap.SetTile(spawnPos, groundTile);
    }
}
