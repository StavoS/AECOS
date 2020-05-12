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
    public float timeDeathByStarve = 100f;
    public float timeDeathByThirst = 100f;

    public float thirst = 0f;
    public float starve = 0f;

    public int timeLived = 0;
    public float pregnantTime = 0;
    public float TimeTilBirth = 10f;

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
    public Animal mate = null;
    
    public bool isInteracting = false;
    
    public Genes genes;
    public bool isMale;
    public bool hasMate = false;

    void Start()
    {
        genes = new Genes();
        mover = GetComponent<AnimalMover>();
        isMale = genes.isMale;
    }

    void Update()
    {
        currentTile = Tile.GetTileAt(transform.position);
        timer += Time.deltaTime;
        if (timer < 5f)
            return;

        starve += Time.deltaTime * 1 / timeDeathByStarve;
        thirst += Time.deltaTime * 1 / timeDeathByThirst;
        timeLived += (int)Time.deltaTime;
        
        if(!isInteracting)
            ChooseNextAct();

        DoAct();
        
        if(starve >= 1f)
        {
            Debug.Log("Died of starvation");
            Environment.DeathAnimal(this);
        }
        else if(thirst >= 1f)
        {
            Debug.Log("Died of thirst");
            Environment.DeathAnimal(this);
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
                if (genes.isMale)
                {
                    if (FindMate())
                    {
                        MoveAnimal(true);
                        return;
                    }
                    _targetTile = Environment.GetNextRandomTile(currentTile);
                    MoveAnimal(false);
                }
                else if (!isInteracting){
                    _targetTile = Environment.GetNextRandomTile(currentTile);
                    MoveAnimal(false);
                }
                else{
                    if(Tile.Distance(currentTile, mate.currentTile) <= 1.505)
                    {
                        _targetTile = mate.currentTile;
                        MoveAnimal(true);
                    }
                }
                break;

            case State.Dead:
                Debug.Log("Dead");
                break;
        }

    }

    public void ChooseNextAct()
    {
        if(genes.reproductiveUrge > starve && genes.reproductiveUrge > thirst)
        {
            act = State.SearchMate;
            return;
        }
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
            food = Environment.GetNearbyGrass(currentTile, genes.sensorDistance);
            
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
            waterTile = Environment.FindClosesetVisibleWater(currentTile, genes.sensorDistance);
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

    public bool FindMate()
    {
        if(mate == null)
            mate = Environment.GetClosesetAnimalBySpieces(this, genes.sensorDistance);

        if (mate == null)
            return false;

        if(!mate.genes.isMale && !mate.hasMate && mate.act == State.SearchMate){
            if (!isInteracting){
                MateFound();
            }
            _targetTile = mate.currentTile;
            return true;
        }
        else if(mate.genes.isMale)
        {
            mate = null;
            return false;
        }
        return true;
    }

    public void MateFound()
    {
        bool accepted = mate.RequestMate(this);
        
        if (accepted){
            isInteracting = true;
            hasMate = true;
            _targetTile = mate.currentTile;
        }
    }

    public bool RequestMate(Animal animal)
    {
        mate = animal;
        isInteracting = true;
        hasMate = true;
        //_targetTile = animal.currentTile;
        mover.SetNewFacing(mate.currentTile);

        return true;
    }
}