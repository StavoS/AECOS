using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    SearchFood,
    SearchWater,
    SearchMate,
    Dead
}
public class Animal : MonoBehaviour
{
    public float sensorDistance = 10f;

    public float timeDeathByStarve = 100f;
    public float timeDeathByThirst = 100f;

    public float thirst = 0f;
    public float starve = 0f;

    public State act;

    public Tile currentTile;
    private Tile _targetTile;

    public AnimalMover mover;
    public LayerMask areaMask;
    public float timer = 0f;

    private const float consumeDuration = 10f;
    private const float drinkConsume = 5f;
    
    public Edible food;
    public Tile waterTile = Tile.Invalid();

    
    public bool isInteracting = false;

    void Start()
    {
        mover = GetComponent<AnimalMover>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 5f)
            return;

        currentTile = Tile.GetTileAt(transform.position);
        starve += Time.deltaTime * 1 / timeDeathByStarve;
        thirst += Time.deltaTime * 1 / timeDeathByThirst;
        
        if(!isInteracting)
            ChooseNextAct();

        DoAct();
        
        if(starve >= 1f)
        {
            Debug.Log("Died of starvation");
        }
        else if(thirst >= 1f)
        {
            Debug.Log("Died of thirst");
        }
    }

    public void DoAct()
    {
        switch (act)
        {
            case State.SearchFood:
                if(FindFood())
                {
                    MoveAnimal(true);
                    return;
                }
                _targetTile = Environment.GetNextRandomTile(currentTile);
                MoveAnimal(false);
                break;

            case State.SearchWater:
                if(FindWater())
                {
                    MoveAnimal(true);
                    return;
                }
                _targetTile = Environment.GetNextRandomTile(currentTile);
                MoveAnimal(false);
                break;

            case State.SearchMate:
                Debug.Log("MATE");
                break;

            case State.Dead:
                Debug.Log("Dead");
                break;
        }

    }

    public void ChooseNextAct()
    {
        if (starve >= thirst)
        {
            act = State.SearchFood;
            return;
        }
        act = State.SearchWater;
    }


    private void MoveAnimal(bool stopBeforeTarget)
    {
        if (!mover.GetPathSet() && !mover.stopBeforeTarget)
            mover.SetPathToTarget(_targetTile, stopBeforeTarget);
    }

    public bool FindFood()
    {
        if (food == null)
        {
            isInteracting = false;
            food = Environment.GetNearbyGrass(currentTile, sensorDistance);
            
            _targetTile = Environment.GetNextRandomTile(currentTile);
           
        }

        if (food != null)
        {
            if(food.OnFocused(this))
            {
                isInteracting = true;
                float amount = Mathf.Min(starve, Time.deltaTime * 1f / consumeDuration);
                amount = food.Eat(amount);
                starve -= amount;
                if(starve <= 0 || food.remaining <= 0)
                {
                    isInteracting = false;
                    food = null;
                    return false;
                }
            }

            _targetTile = Tile.GetTileAt(food.transform.position);

            return true;
        }

        return false;
    }

    public bool FindWater()
    {
        if (waterTile == Tile.Invalid())
        {
            waterTile = Environment.FindClosesetVisibleWater(currentTile, sensorDistance);
        }

        if(waterTile != Tile.Invalid())
        {

            if(Tile.Distance(waterTile, currentTile) <= 1.5f)
            {
                isInteracting = true;
                float amount = Mathf.Min(thirst, Time.deltaTime * 1f / drinkConsume);
                thirst -= amount;
                if(thirst <= 0)
                {
                    isInteracting = false;
                    return false;
                }
                return true;
            }
            if(!isInteracting)
                _targetTile = waterTile;

            return true;
        }

        _targetTile = Environment.GetNextRandomTile(currentTile);
        return false;
    }

}