using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CaveGeneration", menuName = "Generation/Caves", order = 0)]
public class CavesSO : BaseGeneration
{
    CaveGen cave;
    Tunnels tunnels;

    public override void Generate(Tilesmeps world)
    {

        cave = FindFirstObjectByType<CaveGen>().GetComponent<CaveGen>();
        tunnels = FindFirstObjectByType<Tunnels>().GetComponent<Tunnels>();

        //Cave Gen
        int caveWidth = world.width / 8; //Decent size values
        int caveHeight = world.height / 6;

        //Get more or less caves depending on map size and cave size
        int caveCount = (world.width + world.height) / (caveWidth + caveHeight);

        for (int i = 0; i < caveCount; i++)
        {
            //Getting a start position for the cave
            int xPos = UnityEngine.Random.Range(10, world.width - caveWidth);
            int yPos = UnityEngine.Random.Range(10, world.height - caveHeight);
            Vector2 caveOrigin = new Vector2(xPos, yPos);

            //Making each cave increasingly smaller
            caveWidth -= i;
            caveHeight -= i;

            //TODO Use the Clear tile instead of adding a transparent tile
            cave.GenerateCave(caveWidth, caveHeight, caveOrigin, world);

            //UpdateNoiseGrid(xPos, yPos, caveWidth, caveHeight);
        }

        tunnels.GenerateTunnels(world);
    }
}
