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
    private Tile _targetInteract;
    private Tile _faceTile;

    private bool _isTransitioning;
    private bool isSetPath = false;
    public bool stopBeforeTarget = false;
    public bool isFacingProccess = false;

    void Start()
    {
        _path = new Queue<Tile>();
        _location = Tile.GetTileAt(transform.position);
    }
    void Update()
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
        if (!isSetPath && stopBeforeTarget)
            FaceTarget();

        if(isFacingProccess)
        {
            FaceProccess();
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

    void FaceTarget()
    {
        if ((transform.position - _targetInteract.worldPosition) != Vector3.zero){
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_targetInteract.worldPosition - transform.position), turnSpeed * Time.deltaTime);
        }
        else {
            stopBeforeTarget = false;
            return;
        }

        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_targetInteract.worldPosition - transform.position)) <= 0.01f)
            stopBeforeTarget = false;

    }

    public void SetNewFacing(Tile faceTile)
    {
        _faceTile = faceTile;
        isFacingProccess = true;
    }

    private void FaceProccess()
    {
        if ((transform.position - _faceTile.worldPosition) != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_faceTile.worldPosition - transform.position), turnSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_faceTile.worldPosition - transform.position)) <= 0.01f){
            isFacingProccess = false;
            _faceTile = null;
        }
    }

    public void SetPathToTarget(Tile targetTile, bool stopBeforeTarget)
    {
        _path.Clear();

        this.stopBeforeTarget = stopBeforeTarget;
        Vector3 start = transform.position;
        Vector3 target = targetTile.worldPosition;
        Vector3 direction = (target - start).normalized;

        Vector3 currentLocation = start;
        while (Vector3.Distance(currentLocation, target) > 0.5f)
        {
            currentLocation += direction;
            Tile currentTile = Tile.GetTileAt(currentLocation);
            if (!_path.Contains(currentTile))
            {
                if(currentTile.tilePosition != targetTile.tilePosition || !stopBeforeTarget)
                    _path.Enqueue(currentTile);
            }
        }

        if(!stopBeforeTarget)
        {
            if(!_path.Contains(targetTile))
            {
                _path.Enqueue(targetTile);
            }
        }

        if (stopBeforeTarget)
            _targetInteract = targetTile;

        if(_path.Count != 0)
            isSetPath = true;
    }


    public bool GetPathSet()
    {
        return isSetPath;
    }

}