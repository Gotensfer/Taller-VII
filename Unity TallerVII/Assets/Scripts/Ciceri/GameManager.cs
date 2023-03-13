using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    byte[] connectionToken;
    public static GameManager instance = null;
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        if (connectionToken == null)
        {
            //se crea un nuevo token 
            connectionToken = ConnectionTokenUtils.NewToken();
        }
    }

    public void SetConnectionToken(byte[] connectionToken)
    {
        this.connectionToken = connectionToken;
    }
    public byte[] GetconnectionToken()
    {
        return connectionToken;
    }
}
