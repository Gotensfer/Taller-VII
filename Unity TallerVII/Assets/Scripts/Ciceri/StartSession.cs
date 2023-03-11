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

    private void Awake()
    {
        NetworkRunner netwoekRunnerInScene = FindObjectOfType<NetworkRunner>();
        //si ya exite runner se pasara y no se creara otro
        if (netwoekRunnerInScene != null)
            networkRunner = netwoekRunnerInScene;
    }

    private void Start()
    {
        if (networkRunner == null)
        {
            networkRunner = Instantiate(NetworkRunnerPrefab);
            networkRunner.name = "Seccion";
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                var clientTask = InitialNetworkRunner(networkRunner, GameMode.AutoHostOrClient,"TestSession", GameManager.instance.GetconnectionToken(), NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
            }
            print("servidor funcionando");
        }
        
    }

    public void StarHostMigration(HostMigrationToken hostMigrationToken)
    {
        networkRunner = Instantiate(NetworkRunnerPrefab);
        networkRunner.name = "seccion runner -Migrated";
        var clientTask = InitialNetworkRunnerHostMigration(networkRunner,hostMigrationToken);
        print("servidor se migro");
    }
    INetworkSceneManager GetSceneManager(NetworkRunner runner)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();
        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
        return sceneManager;
    }
    // Update is called once per frame
    protected virtual Task InitialNetworkRunner(NetworkRunner runner, GameMode gameMode,string sessionName ,byte[] connectionToken, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = GetSceneManager(runner);
        runner.ProvideInput = true;
        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = sessionName,
            CustomLobbyName = "OurLobbyID",
            Initialized = initialized,
            SceneManager = sceneManager,
            ConnectionToken = connectionToken
        });
    }
    protected virtual Task InitialNetworkRunnerHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        var sceneManager = GetSceneManager(runner);
        runner.ProvideInput = true;
        return runner.StartGame(new StartGameArgs
        {
            SceneManager = sceneManager,
            HostMigrationToken = hostMigrationToken,
            HostMigrationResume = HostMigrationResume,//resumen de la seccion
            ConnectionToken = GameManager.instance.GetconnectionToken()

        }); ; 
    }
    void HostMigrationResume(NetworkRunner runner)
    {
        print("comenzo el hostMIgrationResumen");

        foreach (var resumenObjet in runner.GetResumeSnapshotNetworkObjects())
        {
            if (resumenObjet.TryGetBehaviour<NetworkCharacterControllerPrototype>(out var charaCharacterController))
            {
                runner.Spawn(resumenObjet, position: charaCharacterController.ReadPosition(), rotation: charaCharacterController.ReadRotation(), onBeforeSpawned: (runner, newNObj) =>
                    {
                        newNObj.CopyStateFrom(resumenObjet);


                        if (resumenObjet.TryGetBehaviour<AvatarStats>(out var oldAvatarPlayer))
                        {
                            FindObjectOfType<Spawn>().SetConnectionTokenMapping(oldAvatarPlayer.Token, newNObj.GetComponent<NetworkObject>());
                        }
                    });
            }
        }
        StartCoroutine(CLeanUpHost());

        print("se completo el hostMigration");
    }
    IEnumerator CLeanUpHost()
    {
        yield return new WaitForSeconds(3.5f);

        FindObjectOfType<Spawn>().OnHostMigrationClean();

    }
     public void OnJoinInLobby()
    {
        var clienteTask = JoinLobby();
    }
    private async Task JoinLobby()
    {
        print("Lobby iniciando");
        string lobbyID = "OurLobbyID";

        var result = await networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyID);

        if (!result.Ok)
        {
            print("Joint lobby"+lobbyID);
        }
        else
        {
            print("lobby ok");
        }

    }

    public void CreateGame(string sessionName,string SceneName)
    {
        print("se crea la seccion"+ sessionName +" escena "+ SceneName +"build index "+ SceneUtility.GetBuildIndexByScenePath($"Scenes/Development/Ciceri/MainMap"));
        var clienteTask = InitialNetworkRunner(networkRunner, GameMode.Host,sessionName, GameManager.instance.GetconnectionToken(), NetAddress.Any(), SceneUtility.GetBuildIndexByScenePath($"Scenes/Development/Ciceri/MainMap"),null) ;
    }
    public void JoinGame(SessionInfo sessionInfo)
    {
        print("entrando a la session "+ sessionInfo.Name);
        var client = InitialNetworkRunner(networkRunner, GameMode.Client, sessionInfo.Name, GameManager.instance.GetconnectionToken(), NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

    }
}
