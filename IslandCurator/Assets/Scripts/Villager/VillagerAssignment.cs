using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAssignment : MonoBehaviour
{
    [SerializeField] AssignmentPoint _startingAssignment = null;
    
    Navigator _movement = null;
    AssignmentPoint _assignedTo = null;
    AssignmentPoint _pointIn = null;
    int _numPointsIn = 0;
    
    void OnEnable()
    {
        GrabVillager _grabbableBody = GetComponent<GrabVillager>();
        _grabbableBody.OnGrab += Unassign;
        _grabbableBody.OnLand += Land;
    }

    void OnDisable()
    {
        GrabVillager _grabbableBody = GetComponent<GrabVillager>();
        _grabbableBody.OnGrab -= Unassign;
        _grabbableBody.OnLand -= Land;
    }

    void Start()
    {
        _movement = GetComponent<Navigator>();
        Assign(_startingAssignment);

        _movement.SetBaseNode(_startingAssignment.GetComponent<MapNode>());
        _movement.ReturnToBaseNode();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        _pointIn = other.gameObject.GetComponent<AssignmentPoint>();
        _numPointsIn += 1;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _numPointsIn--;
        if (_numPointsIn <= 0)
        {
            _pointIn = null;
        }
    }

    public void Assign(AssignmentPoint assignment)
    {
        assignment.AssignWorker(this);
        _assignedTo = assignment;
        UpdateBaseNode();
    }
    
    public void Unassign()
    {
        _assignedTo.UnassignWorker(this);
    }

    void Land()
    {
        // Debug.Log(_pointIn.gameObject.name);
        if (_pointIn != null)
        {
            Assign(_pointIn);
        }
        _movement.ReturnToBaseNode();
    }

    void UpdateBaseNode()
    {
        _movement.SetBaseNode(_assignedTo.GetComponent<MapNode>());
    }
}
