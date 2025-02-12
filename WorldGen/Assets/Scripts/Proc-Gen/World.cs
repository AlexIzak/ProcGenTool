using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using static WorldGen;

public class World : MonoBehaviour
{
    //TODO Potentially extend this tilemap to make my own
    [SerializeField] TileBase grassTile, dirtTile, rockTile;

    [SerializeField] Tilemap tilemap;

    private WorldGen worldGen;
    private int[] map;
    private Vector3Int spawnPos;
    private TileBase spawnTile;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Running!");

        worldGen = GetComponent<WorldGen>();

        //map = worldGen.GenerateTerrain();

        //Single value array will do
        int posX = 0;
        int posY = 0;

        for (int i = 0; i < map.Length; i++)
        {
            if (i > 0) posX++; //Ignore the first x increment

            if (posX > worldGen.worldParams.width) //When exceeding width
            {
                posY++; //Go to the next line
                posX = 0; //And start again - kinda like a typewriter when at the end of the page
            }

            spawnPos = new Vector3Int(posX, posY, 0);

            spawnPos.x -= worldGen.worldParams.width / 2;
            spawnPos.y -= worldGen.worldParams.depth;

            //Debug.Log(spawnPos);

            switch (map[i])
            {
                case 0:
                    spawnTile = null;
                    break;

                case 1:
                    spawnTile = grassTile;
                    break;

                case 2:
                    spawnTile = dirtTile;
                    break;

                case 3:
                    spawnTile = rockTile;
                    break;

                default:
                    spawnTile = null;
                    break;
            }

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

    [ContextMenu("Paint")]
    public void Paint()
    {
        tilemap.SetTile(spawnPos, spawnTile);
    }
}
