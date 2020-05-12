using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genes
{
    public float speed;
    public float reproductiveUrge;
    public float sensorDistance;

    public bool isMale;
    
    public Genes()
    {
        isMale = Random.value > 0.5f;
        speed = Random.Range(2f, 8f);
        sensorDistance = Random.Range(3f, 10f);
        reproductiveUrge = Random.Range(0.1f, 0.5f);
    }

    public static void crossOver(Animal partner)
    {

    }
}
