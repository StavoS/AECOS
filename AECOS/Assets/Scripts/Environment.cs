using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Environment : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject animalPrefab;

    public static int size_x = 50;
    public static int size_z = 50;

    public float timer = 0f;
    private bool isMeshBuilt = false;

    public float numGrass = 20f;
    public float numAnimals = 10f;
    
    private TileMap tileMap;
    private static TileMapData mapData;

    private static Tile[] map;
    private static List<Tile> walkableTileMap;
    private static Dictionary<Tile, List<Tile>> walkableNeighborsMap;

    public static List<Edible> edibles;
    private static List<Animal> animals;

    private static System.Random random;
    void Start()
    {
        animals = new List<Animal>();
        edibles = new List<Edible>();
        tileMap = GetComponent<TileMap>();
        walkableNeighborsMap = new Dictionary<Tile, List<Tile>>();
        walkableTileMap = new List<Tile>();
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
            Init(mapData);
        }
    }

    void Init(TileMapData map_data)
    {
        random = new System.Random();
        map = new Tile[size_x * size_z];

        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_z; j++)
            {
                map[i * size_x + j] = Tile.GetTileAt(new Vector3(i, 0, j), map_data.GetTileType(i, j));    //multiplied by tilesize
                walkableNeighborsMap.Add(map[i * size_x + j], GetWalkableNeighbors(map[i * size_x + j]));
                if(map[i * size_x + j].type != 0)
                {
                    walkableTileMap.Add(map[i * size_x + j]);
                }
            }
        }

        SpawnGrassRandomly();
        SpawnAnimals();
    }

    private void SpawnGrassRandomly()
    {
        for (int i = 0; i < numGrass; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0, size_x) + 0.5f, 0, Random.Range(0, size_z) + 0.5f);
            foreach (Tile tile in map)
            {
                if (tile == Tile.GetTileAt(randomPosition) && tile.type != 0 && tile.type != 2)
                {
                    GameObject go = Instantiate(grassPrefab, randomPosition, Quaternion.identity);
                    edibles.Add(go.GetComponent<Edible>());
                }
            }
        }
    }

    private void SpawnAnimals()
    {
        for (int i = 0; i < numAnimals; i++)
        {
            Vector3 randPos = walkableTileMap[Random.Range(0, walkableTileMap.Count - 1)].worldPosition;
            GameObject go = Instantiate(animalPrefab, randPos, Quaternion.identity);
            animals.Add(go.GetComponent<Animal>());
        }
    }

    public static Edible GetNearbyGrass(Tile tile, float maxDistance) //returns invalid if grass isn't nearby
    {
        float minDistance = maxDistance;
        Edible closetGrass = null;
        foreach(var edible in edibles)
        {
            float tileDist = Tile.Distance(tile, Tile.GetTileAt(edible.transform.position));

            if (tileDist <= maxDistance)
            {
                if(minDistance >= tileDist)
                {
                    minDistance = tileDist;
                    closetGrass = edible;
                }
            }
        }
        return closetGrass;
    }

    public static void DeathGrass(Edible grass)
    {
        edibles.Remove(grass);
        Destroy(grass.gameObject);
    }

    public static void DeathAnimal(Animal animal)
    {
        animals.Remove(animal);
        Destroy(animal.gameObject);
    }

    private List<Tile> GetWalkableNeighbors(Tile tile)
    {
        List<Tile> walkableNeighbors = new List<Tile>();

        if (tile.tilePosition.x + 1 < size_x)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x + 1, (int)tile.tilePosition.y + 0) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 1, (int)tile.tilePosition.y + 0) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 1, 0, tile.tilePosition.y + 0)));
            }
        }

        if (tile.tilePosition.y + 1 < size_z)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x + 0, (int)tile.tilePosition.y + 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 0, (int)tile.tilePosition.y + 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 0, 0, tile.tilePosition.y + 1)));
            }
        }
        if (tile.tilePosition.y + 1 < size_z && tile.tilePosition.x + 1 < size_x)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x + 1, (int)tile.tilePosition.y + 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 1, (int)tile.tilePosition.y + 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 1, 0, tile.tilePosition.y + 1)));
            }
        }
        
        
        if (tile.tilePosition.x - 1 > 0)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x - 1, (int)tile.tilePosition.y + 0) == 1 || mapData.GetTileType((int)tile.tilePosition.x - 1, (int)tile.tilePosition.y + 0) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x - 1, 0, tile.tilePosition.y + 0)));
            }
        }
        
        if (tile.tilePosition.y - 1 > 0)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x + 0, (int)tile.tilePosition.y - 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 0, (int)tile.tilePosition.y - 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 0, 0, tile.tilePosition.y - 1)));
            }
        }
        
        if (tile.tilePosition.y - 1 > 0 && tile.tilePosition.x - 1 > 0)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x - 1, (int)tile.tilePosition.y - 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x - 1, (int)tile.tilePosition.y - 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x - 1, 0, tile.tilePosition.y - 1)));
            }
        }

        if (tile.tilePosition.y - 1 > 0 && tile.tilePosition.x + 1 < size_x)
        {
            if(mapData.GetTileType((int)tile.tilePosition.x + 1, (int)tile.tilePosition.y - 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x + 1, (int)tile.tilePosition.y - 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x + 1, 0, tile.tilePosition.y - 1)));
            }
        }

        if (tile.tilePosition.y + 1 < size_z && tile.tilePosition.x - 1 > 0)
        {
            if (mapData.GetTileType((int)tile.tilePosition.x - 1, (int)tile.tilePosition.y + 1) == 1 || mapData.GetTileType((int)tile.tilePosition.x - 1, (int)tile.tilePosition.y + 1) == 2)
            {
                walkableNeighbors.Add(Tile.GetTileAt(new Vector3(tile.tilePosition.x - 1, 0, tile.tilePosition.y + 1)));
            }
        }

        return walkableNeighbors;
    }

    public static Tile GetNextRandomTile(Tile currentTile)
    {
        foreach(Tile tile in map)
        {
            if(tile == currentTile)
            {
                List<Tile> neighbors = walkableNeighborsMap[tile];
                return neighbors[random.Next(neighbors.Count)];
            }
        }
        return null;
    }

    //returns invalid if water tile wasn't found
    public static Tile FindClosesetVisibleWater(Tile tile, float maxDistance)
    {
        float minDistance = maxDistance;
        Tile newTile = Tile.Invalid();
        
        foreach(Tile tileIter in map)
        {
            float tileDistance = Tile.Distance(tile, tileIter);
            if (tileIter.type == 0 && tileDistance <= maxDistance)
            {
                if(minDistance >= tileDistance)
                {
                    minDistance = tileDistance;
                    newTile = tileIter;
                }
            }
        }

        return newTile;
    }

    public static Animal GetClosesetAnimalBySpieces(Animal current, float maxDistance)
    {
        float minDistance = maxDistance;
        Animal closestAnimal = null;
        foreach(Animal animal in animals)
        {
            float animalDist = Tile.Distance(animal.currentTile, current.currentTile);

            if (animalDist != 0)
            {
                if (animalDist <= maxDistance && !animal.isMale)
                {
                    if (minDistance >= animalDist)
                    {
                        minDistance = animalDist;
                        closestAnimal = animal;
                    }
                }
            }
        }
        return closestAnimal;
    }

    public static void AddAnimals()
    {

    }
}
