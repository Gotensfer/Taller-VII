using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;

using Fusion;
using UnityEngine.UI;
using System;

public class SessionInfoList : MonoBehaviour
{
    public TextMeshProUGUI SessionName;
    public TextMeshProUGUI SessionCount;
    public Button joinToServer;

    SessionInfo sessionInfo;
    // Start is called before the first frame update
    public event Action<SessionInfo> OnjoinSession;

    public void setInformation(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;
        SessionName.text = sessionInfo.Name;
        SessionCount.text = $"{sessionInfo.PlayerCount.ToString()}/{sessionInfo.MaxPlayers.ToString()}";
        bool isJoinButton = true;
        if (sessionInfo.PlayerCount>= sessionInfo.MaxPlayers)
            isJoinButton = false;

        joinToServer.gameObject.SetActive(isJoinButton);
       
    }
    public void JoinScene()
    {
        OnjoinSession?.Invoke(sessionInfo);
    }
}
