using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    Tilesmeps world;

    [SerializeField]
    List<BaseGeneration> stages;

    //TODO Add another SO as a container for all the stages so I can have the other ones for testing

    // Start is called before the first frame update
    void Start()
    {
        world = GetComponent<Tilesmeps>();

        foreach (var stage in stages)
        {
            stage.Generate(world);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var stage in stages)
            {
                stage.Generate(world);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            world.tileGrid.ClearAllTiles();
            //ClearMap();
        }
    }
}
