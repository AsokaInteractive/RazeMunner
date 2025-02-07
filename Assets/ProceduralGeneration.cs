using System;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public Vector2Int startCoords, endCoords;
    public GameObject nodePrefab, startNode;
    public Node currentNode;
    public LayerMask nodeMask = 1 << 3;
    public int rows, cols;
    List<Node> nodes;
    public static event Action OnNodeAdded;

    private void Start()
    {
        var node = Instantiate(nodePrefab, new Vector3(startCoords.x, startCoords.y, 0), Quaternion.identity);
        node.transform.SetParent(transform);
        currentNode = node.GetComponent<Node>();
        currentNode.nodeType = Node.NodeType.Start;
        var tempNodes = FindObjectsOfType<Node>();
        foreach (var tempNode in tempNodes)
        {
            nodes.Add(tempNode);
        }
        StartGeneration();
    }
    private void StartGeneration()
    {
        for(int x = 0; x < cols; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                var tempNode = GetNodeAt(x, y);
                if(x==0 && y==0)
                {
                    tempNode.nodeType = Node.NodeType.Start;
                    continue;
                }
                else if(tempNode != null)
                {
                    int rand = UnityEngine.Random.Range(0, 5);
                    if(rand == 0)
                    {
                        var connector = CheckConnectableNode(tempNode);
                        if (connector == null)
                        {
                            tempNode.nodeType = Node.NodeType.End;
                            return;
                        }
                        else
                        {
                            tempNode.AddConnection(connector);
                            connector.AddConnection(tempNode);
                        }
                    }
                }
            }
        }
    }
    private Node GetNodeAt(int xCoord, int yCoord)
    {
        foreach(var node in nodes)
        {
            if(node.transform.position.x == xCoord && node.transform.position.y == yCoord)
                return node;
        }
        return null;
    }
    private bool CompareNodes(Node node1, Node node2)
    {
        return node1 == node2;
    }
    private Node CheckConnectableNode(Node node)
    {
        Node east = GetNodeAt(node.xCoord + 1, node.yCoord);
        Node west = GetNodeAt(node.xCoord - 1, node.yCoord);
        Node north = GetNodeAt(node.xCoord, node.yCoord + 1);
        Node south = GetNodeAt(node.xCoord, node.yCoord - 1);
        if (east != null && !node.CheckConnection(east))
        {
            return east;
        }
        else if (west != null && !node.CheckConnection(west))
        {
            return west;
        }
        else if (north != null && !node.CheckConnection(north))
        {
            return north;
        }
        else if (south != null && !node.CheckConnection(south))
        {
            return south;
        }
        return null;
    }
}
