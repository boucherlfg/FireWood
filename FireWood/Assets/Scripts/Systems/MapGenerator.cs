using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("generate"))
        {
            (target as MapGenerator).Generate();
        }
        if (GUILayout.Button("Next"))
        {
            (target as MapGenerator).PlaceNext();
        }
    }
}
#endif

public class MapGenerator : MonoBehaviour
{
    private AstarPath _astarPath;
    public Tile green;
    public Tile tree;
    public List<Landmark> landmarks;
    public float padding = 10;
    public int treeCount = 10000;
    public float astarNodeSize = 0.4f;
    public bool runOnPlay = false;
    public int index = 0;
    public int startWoodCount = 5;
    public void PlaceNext()
    {
        var alreadyPlaced = FindObjectsOfType<Landmark>();
        AddLandmark(landmarks[index], alreadyPlaced);
        index++;
        if (index >= landmarks.Count) index = 0;
    }
    public Landmark AddLandmark(Landmark landmark, Landmark[] alreadyPlaced)
    {
        // find a position where no other landmark currently is

        var instance = Instantiate(landmark.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Landmark>();
        var other = instance.GetComponent<Landmark>();
        // Rect bound(Landmark l, Vector2 position) => new(l.Bound.position + position, l.Bound.size);
        var direction = Random.insideUnitCircle.normalized;
        for (int i = 0; i < 100000; i++)
        {
            var result = alreadyPlaced.All(x => !x.Overlaps(other));
            if (result) break;
            other.transform.position += (Vector3)direction;
        }

        other.transform.position = (Vector2)Vector2Int.RoundToInt(other.transform.position);


        return instance.GetComponent<Landmark>();
    }
    private void AddGreen(Rect boundingRect)
    {
        var grid = new GameObject("Grid").AddComponent<Grid>();
        grid.transform.SetParent(transform);
        grid.gameObject.layer = gameObject.layer;

        var go = new GameObject("Tilemap")
        {
            tag = "AutoGenerated"
        };
        go.gameObject.layer = gameObject.layer;
        go.transform.SetParent(grid.transform);
        
        var tilemap = go.AddComponent<TilemapRenderer>().GetComponent<Tilemap>();

        for (var i = boundingRect.xMin; i < boundingRect.xMax; i++)
        {
            for (var j = boundingRect.yMin; j < boundingRect.yMax; j++)
            {
                Vector3Int vect = new(Mathf.RoundToInt(i), Mathf.RoundToInt(j));
                tilemap.SetTile(vect, green);
            }
        }
    }
    private void AddTrees(Rect boundingRect)
    {
        var grid = new GameObject("Grid").AddComponent<Grid>();
        grid.transform.SetParent(transform);
        grid.gameObject.layer = gameObject.layer;
        var tilemap = new GameObject("Tilemap").AddComponent<TilemapRenderer>().AddComponent<TilemapCollider2D>().GetComponent<Tilemap>();
        tilemap.gameObject.tag = "AutoGenerated";
        tilemap.GetComponent<TilemapRenderer>().sortingLayerName = "Top";
        tilemap.gameObject.layer = gameObject.layer;
        tilemap.transform.SetParent(grid.transform);

        var allLandmarks = FindObjectsOfType<Landmark>();
        for (int i = 0; i < treeCount; i++)
        {
            Vector3Int vect = new(Mathf.RoundToInt(Random.Range(boundingRect.xMin, boundingRect.xMax)), Mathf.RoundToInt(Random.Range(boundingRect.yMin, boundingRect.yMax)));
            if (allLandmarks.Any(t => t.Contains((Vector3)vect, 0.5f))) continue;

            tilemap.SetTile(vect, tree);
        }
    }
    private void AddWoodPiles()
    {
        var woods = FindObjectsOfType<WoodScript>(true).OrderBy(x => Random.value);
        for (int i = 0; i < startWoodCount + 1; i++)
        {
            woods.ElementAt(i).gameObject.SetActive(true);
        }
    }
    public void Generate()
    {
        _astarPath = GetComponent<AstarPath>();
        Rect boundingRect = new Rect(0, 0, 0, 0);
        foreach (var landmark in landmarks)
        {
            var alreadyPlaced = FindObjectsOfType<Landmark>();
            var result = AddLandmark(landmark, alreadyPlaced);
            boundingRect = result.ExtendBoundingRect(boundingRect);
        }
        boundingRect.min -= 10 * Vector2.one;
        boundingRect.max += 10 * Vector2.one;

        AddGreen(boundingRect);
        AddTrees(boundingRect);
        AddWoodPiles();
        
        var graph = _astarPath.graphs.First() as GridGraph;
        graph.center = boundingRect.center;
        graph.SetDimensions((int)(boundingRect.width / astarNodeSize), 
                            (int)(boundingRect.height / astarNodeSize), 
                            astarNodeSize);
        graph.Scan();
    }
    // Start is called before the first frame update
    public IEnumerator Start()
    {
        if (runOnPlay)
        {
            Generate();
            yield return new WaitForSeconds(0.1f);
            _astarPath.graphs.First().Scan();
        }
    }
}