using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded;

public class Node : MonoBehaviour
{
    public enum NodeType
    {
        Start,
        End,
        Normal
    }
    public NodeType type;
    public bool nodeTouched;

    public void SetNodeTouched()
    {
        nodeTouched = true;
    }
}
