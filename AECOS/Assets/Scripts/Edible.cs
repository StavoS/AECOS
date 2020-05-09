using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour
{
    public float remaining = 1f;
    private float eatSpeed = 3f;

    public float radius;
    private bool isFocus = false;

    public Animal animalSpot;

    public float timeToLive = 20f;
    private float timer = 0f;


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeToLive && !isFocus)
        {
            isFocus = false;
            Destroy(this.gameObject, 1f);
            Environment.DeathGrass(this);
        }

    }

    public bool OnFocused(Animal animal)
    {
        animalSpot = animal;
        isFocus = true;
        float distance = Vector3.Distance(transform.position, animalSpot.transform.position);
        if (distance <= radius + 0.05f)
        {
            return true;
        }

        return false;
    }

    public float Eat(float amount)
    {
        float amountConsumed = Mathf.Min(remaining, amount);
        remaining -= amount * eatSpeed;
        transform.localScale = Vector3.one * remaining;

        if (remaining <= 0)
            Environment.DeathGrass(this);

        return amountConsumed;
    }
}
