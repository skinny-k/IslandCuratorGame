using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] List<MapNode> _nodes = new List<MapNode>();

    [Header("Gizmo Settings")]
    [SerializeField] string _nodeIconFilename = null;
    [SerializeField] bool _allowIconScaling = true;

    public List<MapNode> Nodes
    {
        get => _nodes;
    }

    public void OnDrawGizmosSelected()
    {
        List<MapNode> drawnNodes = new List<MapNode>();
        Gizmos.color = Color.green;
        
        foreach (MapNode node in _nodes)
        {
            foreach (MapNode neighbor in node.NeighborNodes)
            {
                if (!drawnNodes.Contains(neighbor))
                {
                    Gizmos.DrawLine(node.transform.position, neighbor.transform.position);
                }
            }
            
            Gizmos.DrawIcon(node.transform.position, _nodeIconFilename, _allowIconScaling);
            drawnNodes.Add(node);
        }
    }

    public static void FindPath(MapNode start, MapNode finish, out List<MapNode> outputPath)
    {
        start.SetDistance(finish);

        List<MapNode> activeMapNodes = new List<MapNode>();
        List<MapNode> visitedMapNodes = new List<MapNode>();

        activeMapNodes.Add(start);

        while (activeMapNodes.Count > 0)
        {
            activeMapNodes.Sort();
            MapNode checkMapNode = activeMapNodes[0];

            if (checkMapNode == finish)
            {
                visitedMapNodes.Add(checkMapNode);

                outputPath = new List<MapNode>(visitedMapNodes);
                return;
            }

            visitedMapNodes.Add(checkMapNode);
            activeMapNodes.Remove(checkMapNode);

            List<MapNode> walkableMapNodes = GetWalkableMapNodes(checkMapNode, finish);

            foreach (MapNode walkableMapNode in walkableMapNodes)
            {
                if (visitedMapNodes.Contains(walkableMapNode))
                {
                    continue;
                }

                if (activeMapNodes.Contains(walkableMapNode))
                {
                    int index = activeMapNodes.IndexOf(walkableMapNode);
                    MapNode existingMapNode = activeMapNodes[index];

                    if (existingMapNode.distance > checkMapNode.distance)
                    {
                        activeMapNodes.Remove(existingMapNode);
                        activeMapNodes.Add(walkableMapNode);
                    }
                }
                else
                {
                    activeMapNodes.Add(walkableMapNode);
                }
            }
        }

        outputPath = null;
    }

    static List<MapNode> GetWalkableMapNodes(MapNode currentMapNode, MapNode targetMapNode)
    {
        List<MapNode> possibleMapNodes = currentMapNode.NeighborNodes;
        
        foreach (MapNode MapNode in new List<MapNode>(possibleMapNodes))
        {
            if (MapNode.walkable)
            {
                MapNode.SetDistance(targetMapNode);
            }
            else
            {
                possibleMapNodes.Remove(MapNode);
            }
        }

        possibleMapNodes.TrimExcess();

        return possibleMapNodes;
    }
}
