 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static WorldGen;

public class World : MonoBehaviour
{
    //TODO Potentially extend this tilemap to make my own
    public TileBase groundTile;
    public Tilemap tilemap;

    private WorldGen worldGen;
    private int[] map;
    private Vector3Int spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        map = worldGen.GenerateTerrain();

        //Single value array will do
        int posX = 0;
        int posY = 0;

        for (int k = 0; k < map.Length; ++k)
        {
            if (k > 0) ++posX; //Ignore the first x increment

            if (posX > worldGen.worldParams.worldWidth) //When exceeding width
            {
                ++posY; //Go to the next line
                posX = 0; //And start again - kinda like a typewriter when at the end of the page
            }

            spawnPos = new Vector3Int(posX, posY, 0);
            //return pos; //This would return every loop so it would have to run in a loop - not great
            //If I return the array, I would have to just copy the array in a different container

            Paint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Generate")]
    void GenerateMap()
    {
        //Init function
        //Call algorithm functions from WorldGen
    }

    public void Paint()
    {
        tilemap.SetTile(spawnPos, groundTile);
    }
}
