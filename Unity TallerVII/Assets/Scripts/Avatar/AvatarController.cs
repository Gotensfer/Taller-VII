using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkObject))]
public class AvatarController : NetworkBehaviour, INetworkRunnerCallbacks
{
    private Action<Vector2> onMoveAction; public Action<Vector2> OnMoveAction => onMoveAction;
    private Action<bool> onCrouchAction; public Action<bool> OnCrouchAction => onCrouchAction;
    private UnityEvent onJumpAction = new UnityEvent(); public UnityEvent OnJumpAction => onJumpAction;
    private UnityEvent onDashAction = new UnityEvent(); public UnityEvent OnDashAction => onDashAction;
    private UnityEvent onFireAction = new UnityEvent(); public UnityEvent OnFireAction => onFireAction;
    private UnityEvent onReloadAction = new UnityEvent(); public UnityEvent OnReloadAction => onReloadAction;
    private UnityEvent onPickupAction = new UnityEvent(); public UnityEvent OnPickupAction => onPickupAction;

    private bool isCrouched;
    public bool IsCrouched => isCrouched;

    private GeneralInputActions inputActions;

    [Networked] private NetworkButtons previousButtons { get; set; }

    private void OnEnable()
    {
        inputActions = new GeneralInputActions();
        if (Runner != null)
        {
            Debug.Log("OwO");
            EnableInputs();
            Runner.AddCallbacks(this);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<AvatarInput>(out var input)) UseInputs(input);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        AvatarInput localInput = new AvatarInput();
        input.Set(GetInputs(localInput));
    }

    private AvatarInput GetInputs(AvatarInput input)
    {
        // Hay que testear con el sistema de movimiento si esto funciona porque estos datos arrojan un GetButton
        // en vez de un GetButtonDown. La cosa es que no sé exactamente cómo se podría pasar así porque no hay un método
        // que lo haga automático, habría que construir esa detección manualmente, pero por fuera del Set de Photon.
        
        input.Buttons.Set(AvatarButtons.Jump, inputActions.Avatar.Jump.IsPressed());
        input.Buttons.Set(AvatarButtons.Crouch, inputActions.Avatar.Crouch.IsPressed());
        input.Buttons.Set(AvatarButtons.Dash, inputActions.Avatar.Dash.IsPressed());
        input.Buttons.Set(AvatarButtons.Pickup, inputActions.Avatar.Pickup.IsPressed());
        input.DirectionalInput = inputActions.Avatar.Move.ReadValue<Vector2>();
        return input;
    }

    private void UseInputs(AvatarInput input)
    {
        if (input.Buttons.WasPressed(previousButtons, AvatarButtons.Jump)) { OnJumpAction.Invoke(); Debug.Log("Jump Pressed"); }
        if (input.Buttons.WasPressed(previousButtons, AvatarButtons.Crouch)) OnCrouchAction(isCrouched);
        if (input.Buttons.WasPressed(previousButtons, AvatarButtons.Dash)) OnDashAction.Invoke();
        if (input.Buttons.WasPressed(previousButtons, AvatarButtons.Pickup)) OnJumpAction.Invoke();
        //if (input.DirectionalInput != Vector2.zero) OnMoveAction(input.DirectionalInput);     NullReference
        Debug.Log(input.DirectionalInput); // Sí funciona el debug
        // No están funcionando los WasPressed
        Debug.Log(input.Buttons.WasPressed(previousButtons, AvatarButtons.Jump));
        previousButtons = input.Buttons;
    }
    
    private void EnableInputs(){ inputActions.Avatar.Enable(); }
    private void DisableInputs(){ inputActions.Avatar.Disable(); }
    private void OnDisable()
    {
        DisableInputs();
    }

    #region Not implemented interface methods
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    #endregion
}
