using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGeneration : ScriptableObject
{
    protected Tilesmeps tilemap; //This will be the tilemap from the World class

    public abstract void Generate(Tilesmeps world);

    //protected Tilesmeps GetWorld()
    //{
    //    world = FindFirstObjectByType<Tilesmeps>().GetComponent<Tilesmeps>();

    //    return world;
    //}
}
