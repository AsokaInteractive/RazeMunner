using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CandyCoded.HapticFeedback;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Node> allNodes;
    public int nodesTouched;
    public bool roundStarted, canWin,canVibrate = true, playSound = true, darkScreen = true, canChangeLight = true;
    public Image darkScreenPanel, vibrateButton, soundButton, lightButton;
    public Color selectedColor, deselectedColor, screenDarkness;

    public enum PlayerPreferences
    {
        CanVibrate,
        PlaySound,
        DarkScreen
    }

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
        SetPlayerPrefs();
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
    private void SetPlayerPrefs()
    {
        if(PlayerPrefs.HasKey("canVibrate"))
        {
            canVibrate = PlayerPrefs.GetInt("canVibrate") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("canVibrate", canVibrate ? 1 : 0);
        }
        if (PlayerPrefs.HasKey("playSound"))
        {
            playSound = PlayerPrefs.GetInt("playSound") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("playSound", playSound ? 1 : 0);
        }
        if (PlayerPrefs.HasKey("darkScreen"))
        {
            darkScreen = PlayerPrefs.GetInt("darkScreen") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("darkScreen", darkScreen ? 1 : 0);
        }
    }
    public void ToggleVibrate()
    {
        ChangePlayerPrefs(PlayerPreferences.CanVibrate);
    }
    public void ToggleSound()
    {
        ChangePlayerPrefs(PlayerPreferences.PlaySound);
    }
    public void ToggleLight()
    {
        ChangePlayerPrefs(PlayerPreferences.DarkScreen);
    }
    private void ChangePlayerPrefs(PlayerPreferences pp)
    {
        switch(pp)
        {
            case PlayerPreferences.CanVibrate:
                canVibrate = !canVibrate;
                vibrateButton.color = canVibrate ? selectedColor : deselectedColor;
                break;
            case PlayerPreferences.PlaySound:
                playSound = !playSound;
                soundButton.color = playSound ? selectedColor : deselectedColor;
                break;
            case PlayerPreferences.DarkScreen:
                if(!canChangeLight)
                    return;
                darkScreen = !darkScreen;
                lightButton.color = darkScreen ? selectedColor : deselectedColor;
                darkScreenPanel.color = darkScreen ? screenDarkness : Color.clear;
                break;
        }
        SetPlayerPrefs();
    }
    private IEnumerator UpdateScreenBrightness()
    {

        yield return null;
    }

    public void ResetLevel()
    {
        roundStarted = false;
        Player.Instance.canMove = false;
        Player.Instance.transform.position = Vector3.zero;
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
        Invoke(nameof(ResetLevel), 1);
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
