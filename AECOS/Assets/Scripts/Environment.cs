using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Environment : MonoBehaviour
{
    public static int size_x = 50;
    public static int size_z = 50;

    public float timer = 0f;
    private bool isMeshBuilt = false;

    private TileMap tileMap;
    private static TileMapData mapData;

    private static Tile[] map;
    private static Dictionary<Tile, List<Tile>> walkableNeighborsMap;

    private static System.Random random;
    void Start()
    {
        tileMap = GetComponent<TileMap>();
        walkableNeighborsMap = new Dictionary<Tile, List<Tile>>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 1f)
            return;

        if (!isMeshBuilt)
        {
            isMeshBuilt = true;
            mapData = tileMap.BuildMesh();
            Init();
        }
    }

    void Init()
    {
        random = new System.Random();
        map = new Tile[size_x * size_z];
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_z; j++)
            {
                map[i * size_x + j] = Tile.GetTileAt(new Vector3(i, 0, -j));    //multiplied by tilesize
            }
        }

        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_z; j++)
            {
                walkableNeighborsMap.Add(map[i * size_x + j], GetWalkableNeighbors(map[i * size_x + j]));
            }
        }

        List<Tile> noder = new List<Tile>();

        noder = walkableNeighborsMap[map[2322]];
        Debug.Log("tile: " + map[2322].tilePosition);
        foreach (Tile nay in noder)
        {
            Debug.Log("one of neighbors: " + nay.tilePosition);
        }
    }
    public void SpawnGrassRandomly()
    {
        
    }

    public List<Tile> GetWalkableNeighbors(Tile tile)   //Walk to desired food/water through checking act
    {
        List<Tile> walkableNeighbors = new List<Tile>();

        if (tile.tilePosition.x + 1 < size_x)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x + 1, -(int)tile.tilePosition.y + 0) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 1, -(int)tile.tilePosition.y + 0) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 1, 0, tile.tilePosition.y + 0)));
                //Debug.Log(tile.tilePosition.y);
            }
        }

        if (-tile.tilePosition.y + 1 < size_z)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x + 0, -(int)tile.tilePosition.y + 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 0, -(int)tile.tilePosition.y + 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 0, 0, tile.tilePosition.y + 1)));
            }
        }
        if (-tile.tilePosition.y + 1 < size_z && tile.tilePosition.x + 1 < size_x)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x + 1, -(int)tile.tilePosition.y + 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 1, -(int)tile.tilePosition.y + 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 1, 0, tile.tilePosition.y + 1)));
            }
        }
        
        
        if (tile.tilePosition.x - 1 > 0)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x - 1, -(int)tile.tilePosition.y + 0) == 1 || mapData.GetTileType((int)tile.tilePosition.x - 1, -(int)tile.tilePosition.y + 0) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x - 1, 0, tile.tilePosition.y + 0)));
            }
        }
        
        if (-tile.tilePosition.y - 1 > 0)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x + 0, -(int)tile.tilePosition.y - 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 0, -(int)tile.tilePosition.y - 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 0, 0, tile.tilePosition.y - 1)));
            }
        }
        
        if (-tile.tilePosition.y - 1 > 0 && tile.tilePosition.x - 1 > 0)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x - 1, -(int)tile.tilePosition.y - 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x - 1, -(int)tile.tilePosition.y - 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x - 1, 0, tile.tilePosition.y - 1)));
            }
        }

        if (-tile.tilePosition.y - 1 > 0 && tile.tilePosition.x + 1 < size_x)
        {
            if(mapData.GetTileType((int)tile.tilePosition.x + 1, (int)-tile.tilePosition.y - 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 1, (int)-tile.tilePosition.y - 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 1, 0, tile.tilePosition.y - 1)));
            }
        }

        if (-tile.tilePosition.y + 1 < size_z && tile.tilePosition.x - 1 > 0)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x - 1, (int)-tile.tilePosition.y + 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x - 1, (int)-tile.tilePosition.y + 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x - 1, 0, tile.tilePosition.y + 1)));
            }
        }

        return walkableNeighbors;
    }

    public static Tile GetNextRandomTile(Animal animal)
    {
        foreach(Tile tile in map)
        {
            if(tile == animal.currentTile)
            {
                Debug.Log(tile.tilePosition);
                List<Tile> neighbors = walkableNeighborsMap[tile];
                return neighbors[random.Next(neighbors.Count)];
            }
        }
        //List<Tile> neighbors = walkableNeighborsMap[animal.currentTile];
        /*if(!neighbors.Any())
        {
            Debug.Log("ERROR");
        }*/
        return null;
    }

}
