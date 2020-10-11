using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab = default;
    [SerializeField] private Transform obstaclePrefab = default;
    [SerializeField, Range(1f, 10f)] float tileSpeed = 5f;

    public event Action<float> OnDistanceTranslate = delegate { };

    private readonly int countInitTiles = 3;
    private readonly float offsetBetweenTiles = 0.25f;

    private Rect screenRect;
    private Bounds tileBounds;
    private List<Transform> tiles = new List<Transform>();
    private Transform targetTile;
    private List<Vector3> childPositions = new List<Vector3>();

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
        UpdateMotion();
        UpdateCreation();
        UpdateDestruction();
    }

    private void UpdateMotion()
    {
        float dt = Time.deltaTime;
        Vector3 movementDirection = Vector3.back;
        Vector3 velocity = tileSpeed * movementDirection * dt;

        foreach (var tile in tiles)
        {
            tile.position += velocity;
        }

        OnDistanceTranslate?.Invoke(tileSpeed * dt);
    }

    private void UpdateCreation()
    {
        bool isTileAhead = Vector3.Dot(Vector3.back, targetTile.position) < 0f;
        if (isTileAhead || tileBounds.Contains(targetTile.position)) return;

        SpawnTile();

        int targetIndex = tiles.IndexOf(targetTile);
        int nextTargetIndex = Mathf.Clamp(targetIndex + 1, 0, tiles.Count);
        targetTile = tiles[nextTargetIndex];
    }

    private void UpdateDestruction()
    {
        Transform firstTile = tiles[0];
        Vector3 borderPosition = firstTile.position + Vector3.forward * tileBounds.extents.z;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(borderPosition);

        if (!screenRect.Contains(screenPosition))
        {
            Destroy(firstTile.gameObject);
            tiles.RemoveAt(0);
        }
    }

    private void InitTiles()
    {
        for (int i = 0; i < countInitTiles; i++)
        {
            SpawnTile(false);
        }
        targetTile = tiles[0];
    }

    private void SpawnTile(bool withObstacles = true)
    {
        Vector3 newTilePosition = Vector3.zero;
        if (tiles.Count != 0)
        {
            Vector3 lastTilePosition = tiles[tiles.Count - 1].position;
            newTilePosition = lastTilePosition + (tileBounds.size.z + offsetBetweenTiles) * Vector3.forward;
        }

        Transform newTile = Instantiate(tilePrefab, newTilePosition, Quaternion.identity, transform);
        if (withObstacles)
        {
            SpawnObstacles(newTile);
        }

        tiles.Add(newTile);
    }

    private void SpawnObstacles(Transform tile)
    {
        int countObstacles = tile.childCount - 1;
        Vector3 initPosition = tile.position + new Vector3(0f, 0.5f * obstaclePrefab.localScale.y, 0.5f * tileBounds.extents.z);
        var availablePositions = new List<Vector3>(childPositions);

        for (int i = 0; i < countObstacles; i++)
        {
            int index = Random.Range(0, availablePositions.Count);
            Vector3 randomPosition = availablePositions[index];
            availablePositions.RemoveAt(index);

            Vector3 actualPosition = initPosition + randomPosition;
            Transform obstacle = Instantiate(obstaclePrefab, actualPosition, Quaternion.identity, tile);
        }
    }
}