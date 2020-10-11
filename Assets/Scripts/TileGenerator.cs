using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab = default;
    [SerializeField] private Transform obstaclePrefab = default;

    private readonly int countInitTiles = 3;
    private readonly float offsetBetweenTiles = 0.25f;

    private Rect screenRect;
    private Bounds tileBounds;
    private Transform targetTile;
    private List<Vector3> childPositions = new List<Vector3>();

    public List<Transform> Tiles { get; private set; } = new List<Transform>();

    private void Start()
    {
        screenRect = Camera.main.pixelRect;
        tileBounds = BoundsDeterminator.Determine(tilePrefab);

        for (int i = 0; i < tilePrefab.childCount; i++)
        {
            childPositions.Add(tilePrefab.GetChild(i).position);
        }

        InitTiles();
    }

    private void Update()
    {
        UpdateCreation();
        UpdateDestruction();
    }

    private void UpdateCreation()
    {
        bool isTileAhead = Vector3.Dot(Vector3.back, targetTile.position) < 0f;
        if (isTileAhead || tileBounds.Contains(targetTile.position)) return;

        SpawnTile();

        int targetIndex = Tiles.IndexOf(targetTile);
        int nextTargetIndex = Mathf.Clamp(targetIndex + 1, 0, Tiles.Count);
        targetTile = Tiles[nextTargetIndex];
    }

    private void UpdateDestruction()
    {
        Transform firstTile = Tiles[0];
        Vector3 borderPosition = firstTile.position + Vector3.forward * tileBounds.extents.z;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(borderPosition);

        if (!screenRect.Contains(screenPosition))
        {
            Destroy(firstTile.gameObject);
            Tiles.RemoveAt(0);
        }
    }

    private void InitTiles()
    {
        for (int i = 0; i < countInitTiles; i++)
        {
            SpawnTile(false);
        }
        targetTile = Tiles[0];
    }

    private void SpawnTile(bool withObstacles = true)
    {
        Vector3 newTilePosition = Vector3.zero;
        if (Tiles.Count != 0)
        {
            Vector3 lastTilePosition = Tiles[Tiles.Count - 1].position;
            newTilePosition = lastTilePosition + (tileBounds.size.z + offsetBetweenTiles) * Vector3.forward;
        }

        Transform newTile = Instantiate(tilePrefab, newTilePosition, Quaternion.identity, transform);
        if (withObstacles)
        {
            SpawnObstacles(newTile);
        }

        Tiles.Add(newTile);
    }

    private void SpawnObstacles(Transform tile)
    {
        int countObstacles = tile.childCount - 1;
        Vector3 initPosition = tile.position + new Vector3(0f, 0.5f * obstaclePrefab.localScale.y, 0.5f * tileBounds.extents.z);
        var remainingPositions = new List<Vector3>(childPositions);

        for (int i = 0; i < countObstacles; i++)
        {
            int index = Random.Range(0, remainingPositions.Count);
            Vector3 randomPosition = remainingPositions[index];
            remainingPositions.RemoveAt(index);

            Instantiate(obstaclePrefab, initPosition + randomPosition, Quaternion.identity, tile);
        }
    }
}