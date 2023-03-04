using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarMovement : NetworkBehaviour
{
    [SerializeField] float jumpHeight = 1;

    [SerializeField] float crouchSpeed = 0.5f;

    [SerializeField] float crouchHeight = 1.5f;

    [SerializeField] float dashTime = 0.5f;

    [SerializeField] float dashDistance = 2;

    [SerializeField] float movementSpeed = 1;

    NetworkCharacterControllerPrototype cc;

    float standingHeight;

    private void Awake()
    {
        cc = GetComponent<NetworkCharacterControllerPrototype>();

        standingHeight = cc.Controller.height;
    }

    void Move(Vector2 directionalInput)
    {
        Vector3 movementDirection = new Vector3(directionalInput.x, 0, directionalInput.y).normalized;
        Vector3 movementVector = movementDirection * movementSpeed;

        cc.Move(movementVector);
    }

    public override void FixedUpdateNetwork()
    {
        Vector2 inputDirection = new();

        if (Keyboard.current[Key.W].isPressed)
        {
            inputDirection += Vector2.up;
        }

        if (Keyboard.current[Key.S].isPressed)
        {
            inputDirection += Vector2.down;
        }

        if (Keyboard.current[Key.A].isPressed)
        {
            inputDirection += Vector2.left;
        }

        if (Keyboard.current[Key.D].isPressed)
        {
            inputDirection += Vector2.right;
        }

        if (Keyboard.current[Key.Space].isPressed)
        {
            Jump();
        }

        if (Keyboard.current[Key.X].isPressed)
        {
            Crouch(true);
        }

        if (Keyboard.current[Key.C].isPressed)
        {
            Crouch(false);
        }

        Move(inputDirection);
    }

    void Crouch(bool toCrouch)
    {
        if (toCrouch)
        {
            cc.Controller.height = crouchHeight;
            float centerOffset = (2 - crouchHeight)/2;
            cc.Controller.center = new Vector2(0, 0 - centerOffset); 
        }
        else
        {
            cc.Controller.height = 2;
            cc.Controller.center = new Vector2(0, 0);
        }    
    }

    void Jump()
    {
        float gravity = cc.gravity;

        // Calcular la velocidad inicial requerida para el salto
        float initialVelocity = Mathf.Sqrt(2 * -gravity * jumpHeight);

        // Calcular tiempo de ascenso
        float timeToApex = initialVelocity / -gravity;

        // Calculate la velocidad vertical necesaria para la altura del salto indicada
        float jumpVelocity = -gravity * timeToApex;

        cc.Jump(false, jumpVelocity);
    }

    void Dash() // 2m
    {

    }
}
