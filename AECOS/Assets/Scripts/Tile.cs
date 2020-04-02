using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private const float tileSize = 1f;
    private static List<Tile> _tiles = new List<Tile>();
    public Vector2 tilePosition;
    public Vector3 worldPosition;

    private Tile(Vector2 tilePosition)
    {
        this.tilePosition = tilePosition;
        this.worldPosition = new Vector3(this.tilePosition.x * tileSize + 0.5f, 0, this.tilePosition.y * tileSize + 0.5f);

        _tiles.Add(this);
    }

    public static Tile GetTile(Vector2 tilePosition)
    {
        foreach(Tile tile in _tiles)
        {
            if (tile.tilePosition == tilePosition)
                return tile;
        }

        return new Tile(tilePosition);
    }

    public static Tile GetTileAt(Vector3 position)
    {
        Vector2 tilePos = new Vector2(Mathf.Floor(position.x / tileSize), Mathf.Floor(position.z / tileSize));

        return GetTile(tilePos);
    }
}
