using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 2f;
    public float timer = 0f;
    public float moveDelay = 1f;
    public float stopTimer = 0f;

    private Queue<Tile> _path;
    private Tile _location;

    private bool _isTransitioning;
    private bool isSetPath = false;
    private bool isStop = true;

    void Start()
    {
        _path = new Queue<Tile>();
        _location = Tile.GetTileAt(transform.position);
    }
    void Update()
    {
        if (isStop)
        {
            if (isSetPath)
            {
                _isTransitioning = Vector3.Distance(transform.position, _location.worldPosition) > 0.05f; //0.05
                MoveTile();
                if (isSetPath)
                {
                    DoTransition();
                }
            }
        }
    }

    void MoveTile()
    {
        if (_path.Count == 0 || _isTransitioning)
            return;

        _location = _path.Dequeue();
        timer = 0;
    }
    void DoTransition()
    {
        timer += Time.deltaTime;
        if (timer <= moveDelay)
            return;

        transform.position = Vector3.Lerp(transform.position, _location.worldPosition, moveSpeed * Time.deltaTime);
        if ((transform.position - _location.worldPosition) != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_location.worldPosition - transform.position), turnSpeed * Time.deltaTime);
        }
        if (_path.Count == 0 && Vector3.Distance(transform.position, _location.worldPosition) <= 0.05f)
            isSetPath = false;
    }

    public void SetPathToTarget(Tile targetTile)
    {
        if (_isTransitioning)
            return;

        _path.Clear();
        Vector3 start = transform.position;
        Vector3 target = targetTile.worldPosition;
        Vector3 direction = (target - start).normalized;

        Vector3 currentLocation = start;
        while (Vector3.Distance(currentLocation, target) > 0.5f)
        {
            currentLocation += direction;
            Tile currentTile = Tile.GetTileAt(currentLocation);
            //Debug.Log(Vector3.Distance(currentLocation, target));
            if (!_path.Contains(currentTile))
            {
                _path.Enqueue(currentTile);
            }
        }
        _path.Enqueue(targetTile);
        isSetPath = true;
        //Debug.Log(targetTile.worldPosition);
    }


    public bool GetPathSet()
    {
        return isSetPath;
    }

}