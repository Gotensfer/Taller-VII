using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTesting : MonoBehaviour
{
    private GeneralInputActions InputActions;
    private Vector2 movementInput;

    private void Awake()
    {
        InputActions = new GeneralInputActions();
    }

    private void Update()
    {
        var avatarActions = InputActions.Avatar;
        
        if (avatarActions.Jump.IsPressed()) Debug.Log("Jump Pressed");
        if (avatarActions.Crouch.IsPressed()) Debug.Log("Crouch Pressed");
        if (avatarActions.Dash.IsPressed()) Debug.Log("Dash Pressed");
        if (avatarActions.Pickup.IsPressed()) Debug.Log("Pickup Pressed");
        
        Vector2 movementInputValue = avatarActions.Move.ReadValue<Vector2>();
        if (movementInputValue != movementInput) Debug.Log(movementInputValue);
        movementInput = movementInputValue;
    }

    private void OnEnable() { InputActions.Enable(); }
    private void OnDisable() { InputActions.Disable(); }
}
