using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OreGeneration", menuName = "Generation/Ores", order = 1)]
public class OresSO : BaseGeneration
{
    Ore ore;

    public override void Generate(Tilesmeps world)
    {
        ore = FindFirstObjectByType<Ore>().GetComponent<Ore>();

        ore.GenerateOre(world);
    }
}
