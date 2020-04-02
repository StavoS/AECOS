using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject grassPrefab;
    private Vector3 boardSurfaceCenter;
    public int amountOfGrass;
    public float timePerSpawn;
    private float timer = 0f;
    void Start()
    {
        boardSurfaceCenter = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        SpawnGrassRandomly();
    }
    
    public void SpawnGrassRandomly()
    {
        timer += Time.deltaTime;
        if (timer > timePerSpawn)
        {
            for (int i = 0; i < amountOfGrass; i++)
            {
                Instantiate(grassPrefab, boardSurfaceCenter + new Vector3(Mathf.Round(Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2)) + 0.5f, 0, Mathf.Round(Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2)) + 0.5f), Quaternion.identity);
            }
            timer = 0f;
        }
    }

}
