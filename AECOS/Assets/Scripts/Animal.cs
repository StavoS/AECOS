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

    public float health = 100f;
    public float thirst = 0f;
    public float reproductivity = 0f;
    public float starve = 20f;

    public State act;
    private Tile _targetTile;
    public AnimalMover mover;
    public LayerMask areaMask;
    private Collider[] _hitColliders;

    public Edible food;
    private bool _foodFound;
    void Start()
    {
        mover = GetComponent<AnimalMover>();
    }

    void Update()
    {
        _hitColliders = Physics.OverlapSphere(transform.position, sensorDistance, areaMask);
        //Debug.Log(_hitColliders.Length);
        ChooseNextAct();
        DoAct();
    }

    public void DoAct()
    {
        switch (act)
        {
            case State.SearchFood:
                if(FindFood())
                {
                    MoveAnimal();
                }
                break;

            case State.SearchWater:
                Debug.Log("WATER");
                break;

            case State.SearchMate:
                Debug.Log("MATE");
                break;

            case State.Dead:
                Debug.Log("Dead");
                break;
        }
        Ray ray = new Ray(transform.position, (_targetTile.worldPosition - transform.position).normalized);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * Vector3.Distance(transform.position, _targetTile.worldPosition), Color.red);

        if (!mover.GetPathSet())
            MoveAnimal();
        Debug.Log(_targetTile.worldPosition);
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

    public Vector3 ChooseNextRandomTarget()
    {
        Vector3 nextPos = transform.position + new Vector3(Random.Range(-sensorDistance, sensorDistance), 0f, Random.Range(-sensorDistance, sensorDistance));
        return nextPos;
    }

    private void MoveAnimal()
    {
        mover.SetPathToTarget(_targetTile);
    }

    public bool FindFood()
    {
        if (food == null)
        {
            food = CheckFoodNearby();
            if (!mover.GetPathSet())
                _targetTile = Tile.GetTileAt(ChooseNextRandomTarget());
        }

        if (food != null)
        {
            food.OnFocused(this);
            _targetTile = Tile.GetTileAt(food.transform.position);
            return true;
        }

        return false;
    }

    public Edible CheckFoodNearby()
    {
        foreach (Collider col in _hitColliders)
        {
            if (col.gameObject.CompareTag("Edible"))
            {
                return col.gameObject.GetComponent<Edible>();
            }
        }

        return null;
    }

}