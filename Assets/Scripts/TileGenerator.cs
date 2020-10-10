using System.Collections.Generic;

using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab = default;
    [SerializeField, Range(1f, 10f)] float tileSpeed = 5f;

    private readonly int countInitTiles = 3;
    private readonly float offsetBetweenTiles = 0.25f;

    private Bounds tileBounds;
    private List<Transform> tiles = new List<Transform>();
    private Transform targetTile;
    private Rect screenRect;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + tileBounds.center, tileBounds.size);
    }

    private void Start()
    {
        screenRect = Camera.main.pixelRect;
        DetermineTileBounds();
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
        Vector3 direction = Vector3.back;
        Vector3 velocity = tileSpeed * direction * Time.deltaTime;

        foreach (var tile in tiles)
        {
            tile.position += velocity;
        }
    }

    private void UpdateCreation()
    {
        if (tileBounds.Contains(targetTile.position)) return;

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
            SpawnTile();
        }
        targetTile = tiles[0];
    }

    private void SpawnTile()
    {
        Vector3 newTilePosition = Vector3.zero;
        if (tiles.Count != 0)
        {
            Vector3 lastTilePosition = tiles[tiles.Count - 1].position;
            newTilePosition = lastTilePosition + tileBounds.size.z * Vector3.forward;
        }

        Transform newTile = Instantiate(tilePrefab, newTilePosition, Quaternion.identity, transform);
        tiles.Add(newTile);
    }

    private void DetermineTileBounds()
    {
        MeshFilter[] meshes = tilePrefab.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mesh in meshes)
        {
            Bounds meshBounds = mesh.sharedMesh.bounds;
            Transform meshTransform = mesh.transform;

            meshBounds.size = Vector3.Scale(meshBounds.size, meshTransform.localScale) + Vector3.forward * offsetBetweenTiles;
            meshBounds.center += meshTransform.localPosition;

            tileBounds.Encapsulate(meshBounds);
        }
    }
}