using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AvatarController : MonoBehaviour
{
    private Action<Vector2> onMoveAction;
    public Action<Vector2> OnMoveAction => onMoveAction;
    private Action<Vector2> onCrouchAction;
    public Action<Vector2> OnCrouchAction => onCrouchAction;

    private UnityEvent onJumpAction;
    public UnityEvent OnJumpAction => onJumpAction;
    private UnityEvent onDashAction;
    public UnityEvent OnDashAction => onDashAction;
    private UnityEvent onFireAction;
    public UnityEvent OnFireAction => onFireAction;
    private UnityEvent onReloadAction;
    public UnityEvent OnReloadAction => onReloadAction;

    private bool isCrouched;
    public bool IsCrouched => isCrouched;

    private GeneralInputActions inputActions = new GeneralInputActions();

    private void OnEnable()
    {
        EnableInputs();
    }

    private void OnInput(NetworkRunner runner, NetworkInput input)
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
        
    }
    
    private void EnableInputs(){ inputActions.Avatar.Enable(); }
    private void DisableInputs(){ inputActions.Avatar.Disable(); }

    private void OnDisable()
    {
        DisableInputs();
    }
}
