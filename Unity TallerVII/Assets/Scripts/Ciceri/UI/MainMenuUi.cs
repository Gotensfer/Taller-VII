using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{
    public TMP_InputField InputField;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            InputField.text = PlayerPrefs.GetString("PlayerName");
        }
    }
    public void OnJoinGame()
    {
        
        PlayerPrefs.SetString("PlayerName",InputField.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
}
