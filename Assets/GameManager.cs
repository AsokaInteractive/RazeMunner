using System.Collections.Generic;
using UnityEngine;
//using CandyCoded.HapticFeedback;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Node> allNodes;
    public int nodesTouched;
    public bool roundStarted, canWin;

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
        //Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
            Reload();
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
        Invoke(nameof(Reload), 1);
    }
    public void Lose()
    {
        Debug.Log("You Lose!");
        //HapticFeedback.LightFeedback();
        Invoke(nameof(Reload), 1);
    }
    public void StartGame()
    {
        if(roundStarted)
        {
            return;
        }
        Debug.Log("Game Started!");
        roundStarted = true;
        //HapticFeedback.MediumFeedback();
        //Cursor.lockState = CursorLockMode.None;
    }
    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
