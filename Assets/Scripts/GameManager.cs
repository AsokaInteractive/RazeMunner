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
    public bool roundStarted, canWin;
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
    private bool GetPlayerPrefs(PlayerPreferences pp)
    {
        switch (pp)
        {
            case PlayerPreferences.CanVibrate:
                return PlayerPrefs.GetInt("canVibrate") == 1;
            case PlayerPreferences.PlaySound:
                return PlayerPrefs.GetInt("playSound") == 1;
            case PlayerPreferences.DarkScreen:
                return PlayerPrefs.GetInt("darkScreen") == 1;
            default:
                return false;
        }
    }
    private void SetPlayerPrefs()
    {
        if(!PlayerPrefs.HasKey("canVibrate"))
        {
            PlayerPrefs.SetInt("canVibrate", 1);
        }
        if (!PlayerPrefs.HasKey("playSound"))
        {
            PlayerPrefs.SetInt("playSound", 1);
        }
        if (!PlayerPrefs.HasKey("darkScreen"))
        {
            PlayerPrefs.SetInt("darkScreen", 1);
        }
        darkScreenPanel.color = GetPlayerPrefs(PlayerPreferences.DarkScreen) ? screenDarkness : Color.clear;
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
                PlayerPrefs.SetInt("canVibrate", GetPlayerPrefs(PlayerPreferences.CanVibrate) ? 0 : 1);
                vibrateButton.color = GetPlayerPrefs(PlayerPreferences.CanVibrate) ? selectedColor : deselectedColor;
                break;
            case PlayerPreferences.PlaySound:
                PlayerPrefs.SetInt("playSound", GetPlayerPrefs(PlayerPreferences.PlaySound) ? 0 : 1);
                soundButton.color = GetPlayerPrefs(PlayerPreferences.PlaySound) ? selectedColor : deselectedColor;
                break;
            case PlayerPreferences.DarkScreen:
                //if(!canChangeLight)
                //    return;
                PlayerPrefs.SetInt("darkScreen", GetPlayerPrefs(PlayerPreferences.DarkScreen) ? 0 : 1);
                lightButton.color = GetPlayerPrefs(PlayerPreferences.DarkScreen) ? selectedColor : deselectedColor;
                darkScreenPanel.color = GetPlayerPrefs(PlayerPreferences.DarkScreen) ? screenDarkness : Color.clear;
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
        Player.Instance.losePS.Stop();
        Player.Instance.losePS.Clear();
        Player.Instance.transform.position = Vector3.zero;
        //Player.Instance.gameObject.GetComponent<TrailRenderer>().enabled = true;
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
