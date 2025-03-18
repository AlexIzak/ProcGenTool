using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainGeneration", menuName = "Generation/Terrain", order = -1)]
public class TerrainSO : BaseGeneration
{
    UndergroundGen terrain;

    //public GameObject grid;

    public override void Generate(Tilesmeps world)
    {
        terrain = FindFirstObjectByType<UndergroundGen>().GetComponent<UndergroundGen>();

        //world = FindFirstObjectByType<Tilesmeps>().GetComponent<Tilesmeps>();

        terrain.CreateTileset();

        terrain.GenerateUnderground(world);
        terrain.GenerateSurface(world);
    }
}
