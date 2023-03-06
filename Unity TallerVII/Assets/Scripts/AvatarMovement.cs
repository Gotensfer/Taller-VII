using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AvatarController))]
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
        AvatarController avatarController = GetComponent<AvatarController>();

        avatarController.OnMoveAction += Move;
        avatarController.OnJumpAction.AddListener(Jump);
        avatarController.OnCrouchAction += Crouch;
        avatarController.OnDashAction.AddListener(Dash);

        standingHeight = cc.Controller.height;
    }

    void Move(Vector2 directionalInput)
    {
        Vector3 movementDirection = new Vector3(directionalInput.x, 0, directionalInput.y).normalized;
        Vector3 movementVector = movementDirection * movementSpeed * Runner.DeltaTime;

        cc.Move(movementVector);
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
        cc.Velocity = cc.Transform.forward * dashDistance;
        print("Dash?");
    }
}
