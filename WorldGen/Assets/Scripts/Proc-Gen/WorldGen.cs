using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WorldGen : MonoBehaviour
{

    public struct WorldParams
    {
        //Size,
        public int worldWidth;
        public int worldDepth;

        //depth of map (surface - 0 to 20, underground - 20 to 80, cavern - 80+)
        public int surfaceStart;
        public int surfaceLimit;
        public int undergroundLimit;
        public int cavernLimit;
    }
    
    private Vector3Int spawnPos;
    
    public WorldParams worldParams;

    int[] indices = { };

    public int[] GenerateTerrain()
    {
        int index = 0;

        for (int i = 0; i < worldParams.worldWidth; ++i)
        {
            for (int j = 0; j < worldParams.worldDepth; ++j)
            {
                spawnPos.x = i - worldParams.worldWidth / 2;
                spawnPos.y = j - worldParams.worldDepth;


                //Take depth into account
                // If y >= 0 - tile should be grass (account for hills - above 0)

                // If y < 20 - entered underground - more rock

                // If y < 80 - entered underground - mostly rock, more ore

                //Store each value (decides the tile) in an array, index decides position
                indices.SetValue(1, index);
                ++index;

                //Option 1 - only stores location so I would have to figure out tile allocation after - simpler solution found
                //Vector2[] world = { };
                //world.SetValue(new Vector2(spawnPos.x, spawnPos.y), index);
                //index++;

                //Option 2 - 2D array of values (used to decide tile type), the location can be derived from the indices of each value - Overkill
                //int[,] world2 = { };
                //world2.SetValue(1, indices);
            }
        }

        ////Single value array will do
        //int posX = 0;
        //int posY = 0;

        //for (int k = 0; k < indices.Length; ++k)
        //{
        //    if (k > 0) ++posX; //Ignore the first x increment

        //    if (posX > worldParams.worldWidth) //When exceeding width
        //    {
        //        ++posY; //Go to the next line
        //        posX = 0; //And start again - kinda like a typewriter when at the end of the page
        //    }

        //    Vector3Int pos = new Vector3Int(posX, posY, 0);
        //    //return pos; //This would return every loop so it would have to run in a loop - not great
        //    //If I return the array, I would have to just copy the array in a different container

        return indices;
    }

    void GenerateCaves()
    {
        //Algorithm goes here

    }

    void GenerateOre()
    {
        //Algorithm goes here

    }
}
