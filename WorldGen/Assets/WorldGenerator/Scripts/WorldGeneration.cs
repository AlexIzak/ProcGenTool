using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    MyTilemap world;

    //[SerializeField]
    [HideInInspector]
    public List<BaseGeneration> stages = new List<BaseGeneration>();

    //TODO Add another SO as a container for all the stages so I can have the other ones for testing

    //Uncomment if you want runtime generation
    //void Start()
    //{
    //    world = GetComponent<MyTilemap>();

    //    foreach (var stage in stages)
    //    {
    //        stage.Generate(world);
    //    }
    //}

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        foreach (var stage in stages)
    //        {
    //            stage.Generate(world);
    //        }
    //    }
    //    else if (Input.GetMouseButtonDown(1))
    //    {
    //        world.tileGrid.ClearAllTiles();
    //        //ClearMap();
    //    }
    //}

    public void SetStages(BaseGeneration stage)
    {
        stages.Add(stage);
    }

    public List<BaseGeneration> GetStages() { return stages; }

    public void Generate()
    {
        world = GetComponent<MyTilemap>();

        foreach (var stage in stages)
        {
            if (stage == null)
                Debug.LogWarning("Please add a valid stage to the list");
            else
                stage.Generate(world);
        }
    }

    public void Clear()
    {
        if (world.tileGrid != null)
            world.tileGrid.ClearAllTiles();
        else if(world.tileGrid == null)
            Debug.LogWarning("Nothing to clear, please generate a world first");
    }
}
