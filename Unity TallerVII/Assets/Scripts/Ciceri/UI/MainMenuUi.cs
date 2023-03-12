using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{
    [Header("panels")]
    public GameObject playerDetailspanel;
    public GameObject SessionBrowsPanel;
    public GameObject CrearSessionPanel;
    public GameObject StatusPanel;
    [Header("Player setting")]
    public TMP_InputField InputField;
    [Header("New game ")]
    public TMP_InputField SessionName;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            InputField.text = PlayerPrefs.GetString("PlayerName");
        }
    }
    void Panels()
    {
        playerDetailspanel.SetActive(false);
        SessionBrowsPanel.SetActive(false);
        CrearSessionPanel.SetActive(false);
        StatusPanel.SetActive(false);
    }
    public void OnJoinGame()
    {
        
        PlayerPrefs.SetString("PlayerName",InputField.text);
        PlayerPrefs.Save();

        StartSession startSession = FindObjectOfType<StartSession>();

        startSession.OnJoinInLobby();

        Panels();
     
        SessionBrowsPanel.SetActive(true);

        FindObjectOfType<SessionListUISessions>(true).OnLookinForGameSession();
    }
    public void OnCreateNewGameClicked()
    {
        Panels();

        CrearSessionPanel.SetActive(true);
    }

    public void OnStarSessionOnClick()
    {
        StartSession startSession = FindObjectOfType<StartSession>();

        startSession.CreateGame(SessionName.text,"MainMap");

        Panels();

        StatusPanel.SetActive(true);
    }
    public void OnjoiningServer()
    {
        Panels();
        StatusPanel.gameObject.SetActive(true);
    }
  }
