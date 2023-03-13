using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AvatarNetworkTest : NetworkBehaviour, IPlayerLeft
{
    public static AvatarNetworkTest Local { get; set;}
    
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
        }
        else
        {
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;
        }
    }
    
    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority) Runner.Despawn(Object);
    }

    
}
