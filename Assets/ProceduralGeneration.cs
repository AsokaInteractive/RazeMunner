using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralGeneration : MonoBehaviour
{
    public LayerMask nodeMask = 1 << 3;
    public int rows, cols;
    public List<Node> nodes = new List<Node>();
    private Dictionary<Vector2Int, Node> nodeDictionary = new Dictionary<Vector2Int, Node>();

    [Header("Wall Settings")]
    public GameObject wallPrefab; // Prefab for the wall object
    public float wallOffset = 0.5f; // Offset to position walls between nodes

    private void Awake()
    {
        Node[] tempNodes = FindObjectsOfType<Node>();
        foreach (Node node in tempNodes)
        {
            Vector2Int coord = new Vector2Int((int)node.transform.position.x, (int)node.transform.position.y);
            nodeDictionary[coord] = node;
            nodes.Add(node);
        }
    }

    private void Start()
    {
        StartGeneration();
    }

    private void StartGeneration()
    {
        // Ensure at least one end node exists
        bool hasEndNode = false;

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                var tempNode = GetNodeAt(x, y);
                if (tempNode != null) //sanity check
                {
                    if(x == 0 && y == 0)
                    {
                        tempNode.nodeType = Node.NodeType.Start;
                    }
                    var connector = GetConnectableNode(tempNode);
                    if (connector == null)
                    {
                        if (tempNode.xCoord <= 2 && tempNode.yCoord <= 2)
                            Regenerate();
                        tempNode.nodeType = Node.NodeType.End;
                        hasEndNode = true;
                        Debug.Log($"End node at ({tempNode.xCoord}, {tempNode.yCoord})");
                    }
                }
            }
        }

        // If no end node was created, force one
        if (!hasEndNode)
        {
            Node endNode = GetRandomNode();
            while(endNode.xCoord <= 2  && endNode.yCoord <= 2)
            {
                endNode = GetRandomNode();
            }
            endNode.nodeType = Node.NodeType.End;
            Debug.Log($"Forced end node at ({endNode.xCoord}, {endNode.yCoord})");
        }

        // Generate walls between nodes that are not connected
        GenerateWalls();

        // Verify reachability of all nodes
        VerifyReachability();
    }

    private Node GetRandomNode()
    {
        Node randNode = nodes[Random.Range(0, nodes.Count)];
        return randNode;    
    }

    private Node GetNodeAt(int xCoord, int yCoord)
    {
        if (xCoord < 0 || xCoord >= cols || yCoord < 0 || yCoord >= rows)
            return null;

        Vector2Int coord = new Vector2Int(xCoord, yCoord);
        if (nodeDictionary.ContainsKey(coord))
            return nodeDictionary[coord];
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
            availableNodes.Add(east);
        if (west != null && !node.CheckConnection(west))
            availableNodes.Add(west);
        if (north != null && !node.CheckConnection(north))
            availableNodes.Add(north);
        if (south != null && !node.CheckConnection(south))
            availableNodes.Add(south);

        if (availableNodes.Count == 0)
            return null;

        foreach (Node n in availableNodes)
        {
            if (!node.CheckConnection(n))
                validNodes.Add(n);
        }

        if (validNodes.Count == 0)
            return null;

        Node validNode = validNodes[Random.Range(0, validNodes.Count)];
        node.AddConnection(validNode);
        validNode.AddConnection(node);
        Debug.Log($"Connected node at ({node.xCoord}, {node.yCoord}) to ({validNode.xCoord}, {validNode.yCoord})");
        return validNode;
    }

    private void GenerateWalls()
    {
        foreach (Node node in nodes)
        {
            int x = node.xCoord;
            int y = node.yCoord;

            // Check east neighbor
            Node east = GetNodeAt(x + 1, y);
            if (east != null && !node.CheckConnection(east))
            {
                CreateWall(node, east, Direction.East);
            }

            // Check north neighbor
            Node north = GetNodeAt(x, y + 1);
            if (north != null && !node.CheckConnection(north))
            {
                CreateWall(node, north, Direction.North);
            }
        }
    }

    private void CreateWall(Node nodeA, Node nodeB, Direction direction)
    {
        Vector3 wallPosition = Vector3.zero;
        Quaternion wallRotation = Quaternion.identity;

        switch (direction)
        {
            case Direction.East:
                wallPosition = new Vector3(nodeA.transform.position.x + wallOffset, nodeA.transform.position.y, 0);
                wallRotation = Quaternion.Euler(0, 0, 90); // Rotate for vertical wall
                break;
            case Direction.North:
                wallPosition = new Vector3(nodeA.transform.position.x, nodeA.transform.position.y + wallOffset, 0);
                wallRotation = Quaternion.identity; // No rotation for horizontal wall
                break;
        }

        Instantiate(wallPrefab, wallPosition, wallRotation, transform);
    }

    private void VerifyReachability()
    {
        // Use Breadth-First Search (BFS) to check if all nodes are reachable
        HashSet<Node> visited = new HashSet<Node>();
        Queue<Node> queue = new Queue<Node>();

        // Start from the first node (or any node)
        Node startNode = nodes[0];
        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            foreach (Node neighbor in current.connectedNodes)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Check if all nodes were visited
        if (visited.Count != nodes.Count)
        {
            Debug.LogWarning("Not all nodes are reachable! Regenerating...");
            Regenerate();
        }
        else
        {
            Debug.Log("All nodes are reachable.");
        }
    }

    private void Regenerate()
    {
        // Clear all connections and regenerate
        foreach (Node node in nodes)
        {
            node.connectedNodes.Clear();
            node.nodeType = Node.NodeType.Normal; // Reset node type
        }

        // Destroy existing walls
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        StartGeneration();
    }

    private enum Direction
    {
        East,
        North
    }
}