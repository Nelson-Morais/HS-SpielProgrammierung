using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipToeLogic : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    private int width = 10;
    private int depth = 13;


    public float areaWidth = 200;
    public float areaDepth = 200;

    public float spacing = 1;
    // Start is called before the first frame update
    void Start()
    {
        float platformWidth = (areaWidth - (width - 1) * spacing) / width;
        float platformDepth = (areaDepth - (depth - 1) * spacing) / depth;
        ISet<Vector2Int> path = GeneratePath(width, depth);

        for (int zIndex = 0; zIndex < depth; zIndex++)
        {
            for (int xIndex = 0; xIndex < width; xIndex++)
            {

                GameObject platform = Instantiate(platformPrefab, transform);
                platform.transform.localPosition = new Vector3(
                    -areaWidth / 2 + platformWidth / 2 + xIndex * (platformWidth + spacing),
                    0,
                    -areaDepth / 2 + platformDepth / 2 + zIndex * (platformDepth + spacing));
                platform.transform.localScale = new Vector3(platformWidth, 1, platformDepth);
                TipToePlatform tipToePlatform = platform.GetComponent<TipToePlatform>();
                tipToePlatform.isPath = path.Contains(new Vector2Int(xIndex, zIndex));
            }
        }
    }

    private static HashSet<Vector2Int> GeneratePath(int width, int depth)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        int startX = Random.Range(0, width);
        if (!GeneratePathRecurse(width, depth, new Vector2Int(startX, 0), path)){
            Debug.LogError("Path Generation Failed!");
        }
        return path;
    }

    private static bool GeneratePathRecurse(int width, int depth, Vector2Int pos, HashSet<Vector2Int> path)
    {
        // Check weather new Position is valid
        if (pos.x >= width || pos.x < 0 || pos.y >= depth || pos.y < 0 || path.Contains(pos))
        {
            return false;
        }
        if (pos.y == depth - 1)
        {
            path.Add(pos);
            return true;
        }
        if (CountAdjacent(pos, path) > 1)
        {
            return false;
        }
        path.Add(pos);
        Vector2Int[] toExplore = { pos + Vector2Int.up, pos + Vector2Int.down, pos + Vector2Int.left, pos + Vector2Int.right };
        for(int i = toExplore.Length - 1; i > 0; i--)
        {
            int swapI = Random.Range(0, i);
            Vector2Int temp = toExplore[i];
            toExplore[i] = toExplore[swapI];
            toExplore[swapI] = temp;
        }
        foreach(Vector2Int next in toExplore)
        {
            if (GeneratePathRecurse(width, depth,next, path))
            {
                return true;
            }
        }
        path.Remove(pos);
        return false;
    }

    private static int CountAdjacent(Vector2Int pos, HashSet<Vector2Int> path)
    {
        int count = 0;
        if (path.Contains(pos + Vector2Int.up))
        {
            count++;
        }
        if (path.Contains(pos + Vector2Int.down))
        {
            count++;
        }
        if (path.Contains(pos + Vector2Int.left))
        {
            count++;
        }
        if (path.Contains(pos + Vector2Int.right))
        {
            count++;
        }
        return count;
    }
}
