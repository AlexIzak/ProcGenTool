using UnityEngine;

public abstract class BaseGeneration : ScriptableObject
{
    //protected MyTilemap tilemap; //This will be the tilemap from the World class

    public abstract void Generate(MyTilemap world);

    //protected Tilesmeps GetWorld()
    //{
    //    world = FindFirstObjectByType<Tilesmeps>().GetComponent<Tilesmeps>();

    //    return world;
    //}
}
