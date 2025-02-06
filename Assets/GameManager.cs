using System.Collections.Generic;
using UnityEngine;
//using CandyCoded.HapticFeedback;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Node> allNodes;
    public int nodesTouched;
    public bool roundsStarted, canWin;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        allNodes = new List<Node>(FindObjectsOfType<Node>());  
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void NodeTouched(Node node)
    {
        if (!node.nodeTouched)
        {
            node.nodeTouched = true;
            nodesTouched++;
            if (nodesTouched == allNodes.Count)
            {
                canWin = true;
            }
        }
    }
    public void Win()
    {
        Debug.Log("You Win!");
        //HapticFeedback.HeavyFeedback();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Lose()
    {
        Debug.Log("You Lose!");
        //HapticFeedback.LightFeedback();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void StartGame()
    {
        if(roundsStarted)
        {
            return;
        }
        Debug.Log("Game Started!");
        roundsStarted = true;
        //HapticFeedback.MediumFeedback();
        Cursor.lockState = CursorLockMode.None;
    }
}
