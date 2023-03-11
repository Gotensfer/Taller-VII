using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AvatarAim : MonoBehaviour
{
    [SerializeField] private Camera localCamera;
    
    [SerializeField] private Transform cameraAnchorPoint;
    [SerializeField] private float mouseSensibility;
    
    private Vector2 aimInput;
    private Vector2 cameraRotation;

    private Vector3 cameraForward; public Vector3 CameraForward => cameraForward;
    

    private void Awake()
    {
        localCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        if (localCamera.enabled) localCamera.transform.parent = null;
        AvatarController avatarController = GetComponent<AvatarController>();
        avatarController.OnAimAction += GetAimInput;
        LockCursor();
    }
    
    private void LateUpdate()
    {
        if (cameraAnchorPoint == null) return;
        if (!localCamera.enabled) return;
        
        Aim();
    }
    
    private void Aim()
    {
        localCamera.transform.position = cameraAnchorPoint.position;

        cameraRotation.x += aimInput.y * Time.deltaTime * - mouseSensibility;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90, 90);
        cameraRotation.y += aimInput.x * Time.deltaTime * mouseSensibility;
        cameraForward = localCamera.transform.forward;
        localCamera.transform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
    }
    
    private void GetAimInput(Vector2 input, Vector3 forward){ aimInput = input; }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
