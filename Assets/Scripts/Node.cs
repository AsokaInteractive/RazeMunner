using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CandyCoded;

public class Node : MonoBehaviour
{
    public GameObject bNorth, bSouth, bEast, bWest;
    public List<Node> connectedNodes;
    private SpriteRenderer sr;
    public Material normal, end, start;
    public enum BoundaryType
    {
        North,
        South,
        East,
        West
    }
    public enum NodeType
    {
        Start,
        End,
        Normal
    }
    public NodeType nodeType;
    public bool nodeTouched;
    public int xCoord, yCoord;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        xCoord = (int)transform.position.x;
        yCoord = (int)transform.position.y;
    }
    private void Start()
    {
        Invoke(nameof(SetColor), 1);
    }
    private void SetColor()
    {
        if(nodeType == NodeType.Start)
        {
            sr.material = start;
        }
        else if(nodeType == NodeType.End)
        {
            sr.material = end;
        }
    }
    public void SetNodeTouched()
    {
        nodeTouched = true;
    }
    public void AddConnection(Node node, bool remove = false)
    {
        if(remove)        
            connectedNodes.Remove(node);        
        if(!connectedNodes.Contains(node)) //sanity check
        {
            connectedNodes.Add(node);
            DisableBoundary(CheckDirection(node));
        }
    }
    public bool CheckConnection(Node node)
    {
        if(connectedNodes.Contains(node))
            return true;        
        else 
            return false;
    }
    private BoundaryType CheckDirection(Node node)
    {
        if (node.transform.position.y > transform.position.y)
            return BoundaryType.North;
        else if (node.transform.position.y < transform.position.y)
            return BoundaryType.South;
        else if (node.transform.position.x > transform.position.x)
            return BoundaryType.East;
        else if (node.transform.position.x < transform.position.x)
            return BoundaryType.West;
        else
        {
            Debug.LogError("Invalid Direction?");
            return BoundaryType.North;
        }
    }
    public void DisableBoundary(BoundaryType bt, bool enable = false)
    {
        switch(bt)
        {
            case BoundaryType.North:
                if(enable)
                    bNorth.SetActive(true);
                else
                    bNorth.SetActive(false);
                break;
            case BoundaryType.South:
                if (enable)
                    bSouth.SetActive(true);
                else
                    bSouth.SetActive(false);
                break;
            case BoundaryType.East:
                if (enable)
                    bEast.SetActive(true);
                else
                    bEast.SetActive(false);
                break;
            case BoundaryType.West:
                if (enable)
                    bWest.SetActive(true);
                else
                    bWest.SetActive(false);
                break;
        }
    }
}
