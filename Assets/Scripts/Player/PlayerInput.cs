using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private CinemachineCamera cmCamera;
    
    [Header("Movement Settings")]
    [SerializeField] private float mouseSensitivity = 20f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float sprintResponse = 10f;
    [SerializeField] private float jumpStrength = 10f;

    [Header("Camera Settings")]
    [SerializeField] private float turnResponse = 15f;
    private InputSystem inputSystem;
    private float currentSpeed;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        inputSystem = new InputSystem();
        inputSystem.Enable();

        var cmInput = cmCamera.GetComponent<CinemachineInputAxisController>();
    }

    private void Update()
    {
        var input = inputSystem.Player;
        var movement = input.Move.ReadValue<Vector2>();
        var jump = input.Jump.WasPressedThisFrame();
        
        // Set up
        Vector3 moveDir =  new Vector3(movement.x, 0, movement.y);
        moveDir = Vector3.ClampMagnitude(moveDir, 1);
        currentSpeed = Mathf.Lerp(
            a: currentSpeed,
            b: input.Sprint.IsPressed() ? sprintSpeed : moveSpeed,
            t: 1.0f - Mathf.Exp(-sprintResponse * Time.deltaTime)
        );
        
        
        
        // Executing
        if (moveDir.sqrMagnitude >= 0.1f)
        {
            var targetRotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cmCamera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(
                a: transform.rotation,
                b: Quaternion.Euler(0f, targetRotation, 0f),
                t: 1.0f - Mathf.Exp(-turnResponse * Time.deltaTime)
            );
            
            Vector3 rotatedDir = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
            controller.Move(currentSpeed * Time.deltaTime * rotatedDir );
        }

        if (jump)
        {
            controller.Move(Time.deltaTime * jumpStrength * Vector3.up);
        }
        
    }
}
