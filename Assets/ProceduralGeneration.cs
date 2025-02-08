using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralGeneration : MonoBehaviour
{
    public LayerMask nodeMask = 1 << 3;
    public int rows, cols;
    public List<Node> nodes = new List<Node>();

    private void Awake()
    {
        Node[] tempNodes = FindObjectsOfType<Node>();
        if(tempNodes != null )
        {
            foreach (Node node in tempNodes)
            {
                nodes.Add(node);
            }
        }
    }

    private void Start()
    {
        StartGeneration();
    }
    private void StartGeneration()
    {
        for(int x = 0; x < cols; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                var tempNode = GetNodeAt(x, y);
                if(tempNode != null) //sanity check
                {
                    var connector = GetConnectableNode(tempNode);
                    if (connector == null)
                    {
                        tempNode.nodeType = Node.NodeType.End;
                        return;
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
    private Node GetConnectableNode(Node node)
    {
        Node east = GetNodeAt(node.xCoord + 1, node.yCoord);
        Node west = GetNodeAt(node.xCoord - 1, node.yCoord);
        Node north = GetNodeAt(node.xCoord, node.yCoord + 1);
        Node south = GetNodeAt(node.xCoord, node.yCoord - 1);
        List<Node> availableNodes = new List<Node>();
        List<Node> validNodes = new List<Node>();

        if (east != null && !node.CheckConnection(east))
        {
            availableNodes.Add(east);
        }
        if (west != null && !node.CheckConnection(west))
        {
            availableNodes.Add(west);
        }
        if (north != null && !node.CheckConnection(north))
        {
            availableNodes.Add(north);
        }
        if (south != null && !node.CheckConnection(south))
        {
            availableNodes.Add(south);
        }

        bool hasValidNode = false;
        if (availableNodes.Count <= 0)
            return null;
        else
        {
            foreach(Node n in availableNodes)
            {
                if (!node.CheckConnection(n))
                {
                    validNodes.Add(n);
                    hasValidNode = true;
                }
            }
            if(!hasValidNode) 
                return null;
            else
            {
                Node validNode = validNodes[Random.Range(0, validNodes.Count)];
                node.AddConnection(validNode);
                validNode.AddConnection(node);
                return validNode;
            }
        }
    }
}
