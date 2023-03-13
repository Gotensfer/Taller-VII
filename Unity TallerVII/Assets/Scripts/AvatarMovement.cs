using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AvatarController))]
public class AvatarMovement : NetworkBehaviour
{
    [Header("Movement configuration")]
    [SerializeField] float jumpHeight = 1;

    [SerializeField] float crouchHeight = 1.5f;

    [SerializeField] float dashTime = 0.5f;

    [SerializeField] float dashCooldown = 1f;

    [SerializeField] float dashForce = 2;

    [SerializeField] float movementSpeed = 1;

    CustomNetworkCCP cc;

    private AvatarController avatarController;
    
    //Temporal para alfa con capsulas
    // Esto funcionará mientras este LA INTERPOLACION DESACTIVADA del NetworkCharacterControllerPrototype
    // ---
    [SerializeField] GameObject bodyRepresentation;
    // ---

    bool isCrouching = false;
    bool canMove = true;
    bool canDash = true;
    bool canDoubleJump = true;

    float standingHeight;

    float originalSpeed;
    float originalAcceleration;

    private void Awake()
    {
        cc = GetComponent<CustomNetworkCCP>();
        avatarController = GetComponent<AvatarController>();

        cc.maxSpeed = movementSpeed;

        avatarController.OnMoveAction += Move;
        avatarController.OnJumpAction.AddListener(Jump);
        avatarController.OnCrouchAction += Crouch;
        avatarController.OnDashAction += Dash;

        standingHeight = cc.Controller.height;

        originalSpeed = cc.maxSpeed;
        originalAcceleration = cc.acceleration;
    }

    void Move(Vector2 directionalInput, Vector3 forwardVector)
    {
        Vector3 movementVector = Vector3.zero;
        cc.transform.forward = forwardVector;

        if (canMove)
        {
            Vector3 rightMovement = directionalInput.y > 0 ? cc.Transform.forward : Vector3.zero;
            Vector3 leftMovement = directionalInput.y < 0 ? -cc.Transform.forward : Vector3.zero;
            Vector3 forwardMovement = directionalInput.x > 0 ? cc.Transform.right : Vector3.zero;
            Vector3 backwardMovement = directionalInput.x < 0 ? -cc.Transform.right : Vector3.zero;

            Vector3 movementDirection = (rightMovement + leftMovement + forwardMovement + backwardMovement).normalized;
            movementVector = movementDirection * movementSpeed * Runner.DeltaTime;
        }

        Quaternion rotation = cc.transform.rotation;
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
        cc.transform.rotation = rotation;
        
        // Al CharacterControllerProtoype debe llamarse su Move() en todo momento aunque sea un Vector.Zero o de lo contrario
        // toda la simulación del movimiento en este objeto parará, resultando en comportamiento anomalo durante un salto o dash
        cc.Move(movementVector);
    }

    void Crouch(bool toCrouch)
    {
        if (!avatarController.IsCrouched)
        {
            cc.Controller.height = crouchHeight;
            float centerOffset = (2 - crouchHeight)/2;
            cc.Controller.center = new Vector2(0, 0 - centerOffset);
            avatarController.IsCrouched = true;

            //Temporal para alfa con capsulas
            // Esto funcionará mientras este LA INTERPOLACION DESACTIVADA del NetworkCharacterControllerPrototype
            bodyRepresentation.transform.localScale = new Vector3(1, 0.75f, 1);
            bodyRepresentation.transform.localPosition = new Vector3(0, -0.25f, 0);
        }
        else
        {
            cc.Controller.height = 2;
            cc.Controller.center = new Vector2(0, 0);
            avatarController.IsCrouched = false;

            //Temporal para alfa con capsulas
            // Esto funcionará mientras este LA INTERPOLACION DESACTIVADA del NetworkCharacterControllerPrototype
            bodyRepresentation.transform.localScale = Vector3.one;
            bodyRepresentation.transform.localPosition = Vector3.zero;
        }    
    }

    void Jump()
    {
        float gravity = cc.gravity;

        // Calcular la velocidad inicial requerida para el salto
        float initialVelocity = Mathf.Sqrt(2 * -gravity * jumpHeight);

        // Calcular tiempo de ascenso
        float timeToApex = initialVelocity / -gravity;

        // Calcular la velocidad vertical necesaria para la altura del salto indicada
        float jumpVelocity = -gravity * timeToApex;

        // Solo intentar consumir el doble salto si no se está en el suelo
        if (cc.IsGrounded)
        {
            cc.Jump(false, jumpVelocity);
        }
        else if (canDoubleJump)
        {
            cc.Jump(true, jumpVelocity);
            canDoubleJump = false;
        }
    }

    // Hack relativamente liviano para resetear el doble salto
    // si, está en un update, pero no es un proceso pesado - Juanfer
    private void Update()
    {
        if (cc.IsGrounded) canDoubleJump = true;
    }

    void Dash(Vector3 forward)
    {
        if (canDash)
        {
            // Para emular un dash es necesario acomodar los valores del cc para poder
            // aplicar las velocidades necesarias
            
            cc.dashDistance = dashForce;
            cc.dashAcceleration = dashForce;

            canMove = false;
            canDash = false;

            cc.Dash(forward);

            Invoke(nameof(EnableMovement), dashTime);
            Invoke(nameof(ResetDashCD), dashCooldown);
        }
    }

    void EnableMovement()
    {
        canMove = true;
    }

    void ResetDashCD()
    {
        canDash = true;
    }
}
