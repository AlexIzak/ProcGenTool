using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "OreGeneration", menuName = "Generation/Ores", order = 1)]
public class OresSO : BaseGeneration
{
    public int maxClusterSize = 1;

    [Header("This curve controls the ore distribution along the depth of the map")]
    public AnimationCurve heightDist;

    //Old method
    //int[,] oreMap;
    //[Range(0f, 0.9f)]
    //public float decay = 0.5f;
    //int visited = -1;
    //int filled = 1;
    //List<Vector2> queue;
    //Vector2 coords;

    int width, height;

    //TODO Add an ID for each tile using a List of strings by extending the Tilebase class
    [Header("The tile used for the ore veins")]
    public MyTile ore;

    public override void Generate(Tilesmeps world)
    {
        GenerateOre(world);
    }

    public void GenerateOre(Tilesmeps world)
    {
        this.width = world.GetWidth();
        this.height = world.GetHeight();
        //oreMap = new int[width, height];

        int clumpCount = (width * height) / 100;
        //int clumpCount = 25;

        for (int i = 0; i < clumpCount; i++)
        {
            //Making sure the ores ar at least a bit away from the edge of the map
            int startX = UnityEngine.Random.Range(10, width - 10);
            int startY = UnityEngine.Random.Range(10, height - 10);

            //LazyFloodFill(startX, startY);
            
            //Calculating ore amount based off the animation curve
            int clusterSize = (int)(heightDist.Evaluate(startY / (float)height) * maxClusterSize);

            if(clusterSize > 0 && world.GetTile(startX, startY) != null)
                FloodFillOre(startX, startY, clusterSize, world);

            //Testing
            //Debug.Log($"X : {startX}, Y : {startY}, cluster : {clusterSize}");
        }
    }

    //Floodfill algorithm - Old method
    //void LazyFloodFill(int x, int y)
    //{
    //    float chance = 100f;

    //    float depth = (float)height / (float)y;

    //    //decay = Normalize(depth);

    //    decay = Mathf.Clamp(decay, 0f, 0.9f);

    //    queue = new List<Vector2>();
    //    queue.Add(new Vector2(x, y));
    //    while (queue.Count != 0)
    //    {
    //        coords = queue[0];
    //        queue.RemoveAt(0);
    //        oreMap[(int)coords.x, (int)coords.y] = filled;

    //        if (chance >= UnityEngine.Random.Range(1, 100))
    //        {
    //            HandleNeighbours();
    //            chance = chance * decay;
    //        }
    //    }
    //}

    //private float Normalize(float v)
    //{
    //    return v - 1f / ((float)height / 10f) - 1f;
    //}

    //private void HandleNeighbours()
    //{
    //    ValidateandAddtoQueue((int)coords.x, (int)coords.y - 1);

    //    ValidateandAddtoQueue((int)coords.x, (int)coords.y + 1);

    //    ValidateandAddtoQueue((int)coords.x - 1, (int)coords.y);

    //    ValidateandAddtoQueue((int)coords.x + 1, (int)coords.y);
    //}

    private bool IsWithinBounds(float x, float y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) { return false; }

        return true;
    }

    //private void ValidateandAddtoQueue(int x, int y)
    //{
    //    if (IsWithinBounds(x, y) && oreMap[x, y] != filled && oreMap[x, y] != visited)
    //    {
    //        queue.Add(new Vector2(x, y));
    //        oreMap[x, y] = visited;
    //    }
    //}

    private void FloodFillOre(int x, int y, int count, Tilesmeps world)
    {

        int counter = 0;

        List<Vector2Int> frontier = new();

        frontier.Add(new Vector2Int(x,y));


        while (counter < count && frontier.Count > 0)
        {
            // Pick random frontier pos
            int frontierIndex = Random.Range(0, frontier.Count);
            Vector2Int frontierPos = frontier[frontierIndex];
            // Remove picked pos from frontier
            frontier.RemoveAt(frontierIndex);

            world.SetTile(frontierPos.x, frontierPos.y, ore);
            counter++;

            List<Vector2Int> validNeighbours = FindValidNeighbours(frontierPos, world);
            if (validNeighbours.Count > 0)
            {
                // Add found valid neighbours to frontier
                foreach (var neighbour in validNeighbours)
                {
                    if(!frontier.Contains(neighbour))
                    {
                        frontier.Add(neighbour);
                    }
                }
            }
        }
    }

    private List<Vector2Int> FindValidNeighbours(Vector2Int startPos, Tilesmeps world)
    {
        List<Vector2Int> valid = new();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int currentPos = startPos + new Vector2Int(x, y);

                if (Mathf.Abs(x) == Mathf.Abs(y)) // Skip diagonals and start
                    continue;
                if (!IsWithinBounds(currentPos.x, currentPos.y)) // Skip out of bounds
                    continue;
                if (world.GetTile(x, y) == null)// && world.GetTile(x,y).Tags.Contains("Hollow")) // Skip air
                    continue;

                valid.Add(currentPos);
            }

        return valid;
    }
}
