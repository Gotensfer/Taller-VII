using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    Dictionary<int, NetworkObject> mapTokenIDWithNetworkPlayer = new Dictionary<int, NetworkObject>();

    int GetPlayerToken(NetworkRunner runner, PlayerRef player)
    {
        if (runner.LocalPlayer == player)
        {
            return ConnectionTokenUtils.HashToken(GameManager.instance.GetconnectionToken());
        }
        else
        {
            var token = runner.GetPlayerConnectionToken(player);
            if (token == null)
            {
                return ConnectionTokenUtils.HashToken(token);
            }
            Debug.LogError("esto no trae el token");
            return 0;
        }
    }

    public void SetConnectionTokenMapping(int token , NetworkObject networkObject )
    {
        mapTokenIDWithNetworkPlayer.Add(token,networkObject);
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        print("migracion de host");
        await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

        //cambiar el antiguo runner por otro
        FindObjectOfType<StartSession>().StarHostMigration(hostMigrationToken);
    }
    private void Update()
    {
      
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            int playerToken = GetPlayerToken(runner,player);
            Debug.Log("token "+ playerToken);



            if (mapTokenIDWithNetworkPlayer.TryGetValue(playerToken, out NetworkObject networkObject))
            {
                Debug.Log("coneccion a una vieja host ");
                networkObject.GetComponent<NetworkObject>().AssignInputAuthority(player);
            }
            else
            {


                // Create a unique position for the player
                Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
                // Keep track of the player avatars so we can remove it when they disconnect
                _spawnedCharacters.Add(player, networkPlayerObject);
                //si se llega a hacer con avatarStarts y no con playerStats
                //networkPlayerObject.GetComponent<PlayerStats>().GetComponent<AvatarStats>().Token = playerToken;
                networkPlayerObject.GetComponent<AvatarStats>().Token = playerToken;
                //si se va a hacer con el playerStats y no directamente 
               // networkPlayerObject.GetComponent<PlayerStats>().Token = playerToken;
                mapTokenIDWithNetworkPlayer[playerToken] = networkPlayerObject;

            }
        }
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
    public void OnHostMigrationClean()
    {
        foreach (KeyValuePair<int, NetworkObject> entry in mapTokenIDWithNetworkPlayer)
        {
            NetworkObject networkObject = entry.Value.GetComponent<NetworkObject>();
            if (networkObject.InputAuthority.IsNone)
            {
                networkObject.Runner.Despawn(networkObject);
            }
        }
    }

}
