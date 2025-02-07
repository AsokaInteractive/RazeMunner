using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CandyCoded;

public class Node : MonoBehaviour
{
    public GameObject bNorth, bSouth, bEast, bWest;
    public List<Node> connectedNodes;
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
        xCoord = (int)transform.position.x;
        yCoord = (int)transform.position.y;
    }

    public void SetNodeTouched()
    {
        nodeTouched = true;
    }
    public void AddConnection(Node node, bool remove = false)
    {
        if(remove)        
            connectedNodes.Remove(node);        
        else if(!connectedNodes.Contains(node))
            connectedNodes.Add(node);
    }
    public bool CheckConnection(Node node)
    {
        if(connectedNodes.Contains(node)) return true;
        else return false;
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
