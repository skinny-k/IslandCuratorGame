using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _nodeSnapDistance = 0.5f;
    
    List<MapNode> _activePath = null;
    MapNode _baseNode = null;
    int _nodeInPath = 0;

    void Start()
    {
        _baseNode = GetNearestMapNode();
        GetPathToNode(_baseNode);
    }
    
    MapNode GetNearestMapNode()
    {
        List<MapNode> nodes = new List<MapNode>(FindObjectsOfType<MapNode>());
        MapNode nearestNode = null;

        foreach (MapNode node in nodes)
        {
            if (nearestNode != null)
            {
                float distToNearestNode = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(nearestNode.transform.position.x, nearestNode.transform.position.y));
                float distToCurrentNode = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(node.transform.position.x, node.transform.position.y));

                if (distToCurrentNode < distToNearestNode)
                {
                    nearestNode = node;
                }
            }
            else
            {
                nearestNode = node;
            }
        }

        return nearestNode;
    }

    public void GetPathToNode(MapNode target)
    {
        Map.FindPath(GetNearestMapNode(), target, out _activePath);
    }

    public bool ReturnToBaseNode()
    {
        if (_baseNode != null)
        {
            GetPathToNode(_baseNode);
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        if (_activePath != null)
        {
            Vector2 targetPositionWithoutZ = new Vector2(_activePath[_nodeInPath].transform.position.x, _activePath[_nodeInPath].transform.position.y);
            Vector3 targetPosition = new Vector3(targetPositionWithoutZ.x, targetPositionWithoutZ.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);

            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), targetPositionWithoutZ) <= _nodeSnapDistance)
            {
                _nodeInPath++;
            }
            if (_nodeInPath >= _activePath.Count)
            {
                _activePath = null;
                _nodeInPath = 0;
            }
        }
    }
}
