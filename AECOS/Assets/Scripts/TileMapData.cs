using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapData
{
    private int _size_x;
    private int _size_z;
    private int[,] map_data;

    private List<Lake> Lakes;
    public TileMapData(int size_x, int size_z)
    {
        _size_x = size_x;
        _size_z = size_z;

        map_data = new int[size_x, size_z];
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_z; j++)
            {
                map_data[i, j] = 1;
            }
        }
        Lakes = new List<Lake>();

        for (int i = 0; i < 10; i++)
        {
            Lake Lake = new Lake();

            Lake.width = Random.Range(4, 8);
            Lake.height = Random.Range(4, 8);

            Lake.left = Random.Range(0, size_x - Lake.width - 1);
            Lake.top = Random.Range(0, size_z - Lake.width - 1);

            if (!LakeCollides(Lake))
            {
                Lakes.Add(Lake);
            }
        }

        foreach (Lake r in Lakes)
        {
            MakeLake(r);
        }
    }

    public bool LakeCollides(Lake r)
    {
        foreach (Lake r2 in Lakes)
        {
            if (r.CollidesWith(r2))
            {
                return true;
            }
        }

        return false;
    }
    public int GetTileAt(int x, int z)
    {
        return map_data[x, z];
    }

    public void MakeLake(Lake r)
    {
        int changeChance;

        for (int x = 0; x < r.width; x++)
        {
            for (int z = 0; z < r.height; z++)
            {
                changeChance = Random.Range(1, 10);
                if (x == 0 || x == r.width - 1 || z == 0 || z == r.height - 1)
                {

                    map_data[x + r.left, z + r.top] = 2;

                }
                else if (map_data[x + r.left, z + r.top] != 2)
                {
                    map_data[x + r.left, z + r.top] = 0;
                }
            }
        }
    }
}
