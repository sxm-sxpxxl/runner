using System;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(TileGenerator))]
public class TileMotionController : MonoBehaviour
{
    [SerializeField, Range(0f, 50f)] float speed = 20f;

    public event Action<float> OnDistanceAdd = delegate { };

    private IEnumerable<Transform> tiles;
    private Vector3 movementDirection = Vector3.back;

    private void Awake()
    {
        tiles = GetComponent<TileGenerator>().Tiles;
    }

    private void Update()
    {
        if (tiles == null) return;

        float dt = Time.deltaTime;
        Vector3 velocity = speed * movementDirection * dt;

        foreach (var tile in tiles)
        {
            tile.position += velocity;
        }

        OnDistanceAdd?.Invoke(speed * dt);
    }
}