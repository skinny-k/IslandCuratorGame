using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour, IComparable
{
    [SerializeField] int _index;
    [SerializeField] List<MapNode> _neighborNodes = new List<MapNode>();

    public List<MapNode> NeighborNodes
    {
        get => _neighborNodes;
    }

    public int Index
    {
        get => _index;
    }

    public bool walkable;
    public float distance = 0f;

    void OnDrawGizmosSelected()
    {
        Map map = transform.parent.GetComponent<Map>();
        if (map != null)
        {
            map.OnDrawGizmosSelected();
        }
    }

    public int CompareTo(object obj)
    {
        MapNode node = obj as MapNode;
        
        if (node == null)
        {
            return 1;
        }
        else
        {
            if (node.distance < distance)
            {
                return 1;
            }
            else if (node.distance > distance)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public void SetDistance(MapNode node)
    {
        distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(node.transform.position.x, node.transform.position.y));
    }
}
