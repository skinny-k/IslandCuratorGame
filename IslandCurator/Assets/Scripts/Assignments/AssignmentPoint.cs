using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentPoint : MonoBehaviour
{
    [SerializeField] MapNode _mapNode = null;

    int _workerCount = 0;
    
    protected void Start()
    {
        if (_mapNode == null)
        {
            _mapNode = GetComponent<MapNode>();
        }
    }
    
    public MapNode GetNode()
    {
        return _mapNode;
    }
    
    public void AssignWorker(VillagerAssignment worker)
    {
        _workerCount++;
    }

    public void UnassignWorker(VillagerAssignment worker)
    {
        _workerCount--;
    }
}
