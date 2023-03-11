using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SessionListUISessions : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public GameObject SessionListUIPrefab;
    public VerticalLayoutGroup VarticalLayouGroup;

    void clearlist()
    {
        foreach (Transform child in VarticalLayouGroup.transform)
        {
            Destroy(child.gameObject);
        }
        statusText.gameObject.SetActive(false);
    }
    public void AddList(SessionInfo sessionInfo)
    {
        SessionInfoList addSessionOnInfoListUi = Instantiate(SessionListUIPrefab, VarticalLayouGroup.transform).GetComponent<SessionInfoList>();
        addSessionOnInfoListUi.setInformation(sessionInfo);

        addSessionOnInfoListUi.OnjoinSession += AddSessionOnInfoList_OnSession;
    }
    private void AddSessionOnInfoList_OnSession(SessionInfo obj)
    {

    }
    public void onNoSessionFound()
    {
        statusText.text = "no hay sala de juego  ";
        statusText.gameObject.SetActive(true);

    }
    public  void OnLookinForGameSession()
    {
        statusText.text = "mira para secciones";
        statusText.gameObject.SetActive(true);
    }
}
