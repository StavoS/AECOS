using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour
{
    public float radius;
    private bool isFocus = false;
    public Animal animalSpot;

    void Start()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    void Update()
    {
        if (isFocus)
        {
            float distance = Vector3.Distance(transform.position, animalSpot.transform.position);
            if (distance <= radius + 0.05f)
            {
                isFocus = false;
                Debug.Log("noder neder");
                Destroy(this.gameObject, 5f);
            }
        }
    }

    public void OnFocused(Animal animal)
    {
        animalSpot = animal;
        isFocus = true;
    }
}
