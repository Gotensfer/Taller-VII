using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Linq;
using System.Threading.Tasks;
using System;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.SceneManagement;
public class StartSession : MonoBehaviour
{
    [SerializeField] public NetworkRunner NetworkRunnerPrefab;
    NetworkRunner networkRunner;

    [Networked] public NetworkString<_32> Name { get; set; }
    [Networked] public Color Color { get; set; }
    [Networked] public NetworkBool Ready { get; set; }
    [Networked] public NetworkBool DoneLoading { get; set; }

    private void Start()
    {
        networkRunner = Instantiate(NetworkRunnerPrefab);
        networkRunner.name = "Seccion";
        var clientTask = InitialNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
        print("funciona");
    }
    // Update is called once per frame
    protected virtual Task InitialNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();
        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
        runner.ProvideInput = true;
        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "TestRoom",
            Initialized = initialized,
            SceneManager = sceneManager
        });
    }
}
