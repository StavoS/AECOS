using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private const float tileSize = 1f;
    public Vector2 tilePosition;
    public Vector3 worldPosition;

    public bool isWalkable;

    private Tile(Vector2 tilePosition)
    {
        this.tilePosition = tilePosition;
        this.worldPosition = new Vector3(this.tilePosition.x * tileSize + 0.5f, 0, this.tilePosition.y * tileSize + 0.5f);
    }

    public static Tile GetTile(Vector2 tilePosition)
    {
        return new Tile(tilePosition);
    }

    public static Tile GetTileAt(Vector3 position)
    {
        Vector2 tilePos = new Vector2(Mathf.Floor(position.x / tileSize), Mathf.Floor(position.z / tileSize));
        return GetTile(tilePos);
    }

    public static bool operator ==(Tile a, Tile b)
    {
        return a.tilePosition == b.tilePosition;
    }

    public static bool operator !=(Tile a, Tile b)
    {
        return a.tilePosition != b.tilePosition;
    }

    public override bool Equals(object other)
    {
        return (Tile)other == this;
    }
    public override int GetHashCode()
    {
        return 0;
    }
}
