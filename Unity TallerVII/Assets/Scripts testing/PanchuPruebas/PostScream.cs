using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostScream : NetworkBehaviour
{
    [SerializeField] private AK.Wwise.Event test;

    public void PostEvent()
    {
        test.Post(gameObject);
    }
}
