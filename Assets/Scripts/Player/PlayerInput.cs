using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private CinemachineCamera cmCamera;
    [SerializeField] private Animator animator;
    
    [Header("Movement Settings")]
    [SerializeField] private float mouseSensitivity = 20f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float sprintResponse = 10f;
    [SerializeField] private float maxFallSpeed = 20f;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpStrength = 10f;
    [SerializeField] private float gravity = 20.0f;
    [SerializeField] private float airAcceleration = 70f;
    [SerializeField] private float maxAirSpeed = 20f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    [Header("Camera Settings")]
    [SerializeField] private float turnResponse = 15f;
    
    public bool IsGrounded => controller.isGrounded;
    
    private InputSystem inputSystem;
    private PickUpItems pickUp;
    private float currentSpeed;
    private Vector3 currentVelocity;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    
    private bool wasUngroundedByJump;
    private float timeSinceUngrounded;
    private float timeSinceJumpRequest;

    private bool requestedJump = false;

    private void Awake()
    {
        inputSystem = new InputSystem();
        inputSystem.Enable();
    }

    private void Start()
    {
        requestedJump = false;
        Cursor.lockState = CursorLockMode.Locked;
        pickUp = GetComponent<PickUpItems>();
        var cmInput = cmCamera.GetComponent<CinemachineInputAxisController>();
    }

    private void Update()
    {
        // Initial values
        var input = inputSystem.Player;
        var movement = input.Move.ReadValue<Vector2>();
        var jump = input.Jump.WasPressedThisFrame();
        var click = input.Attack.WasPressedThisFrame();
        var rightClick = input.Aim.WasPressedThisFrame();
        var interact = input.Interact.WasPressedThisFrame();
        
        bool wasRequestedJump = requestedJump;
        requestedJump = requestedJump || jump;
        
        if (requestedJump && !wasRequestedJump)
        {
            timeSinceJumpRequest = 0;
        }        
        
        // Setup Inputs
        Vector3 moveDir =  new Vector3(movement.x, 0, movement.y);
        moveDir = Vector3.ClampMagnitude(moveDir, 1);
        
        
        // Executing
        if (interact)
        {
            StartCoroutine(ToggleTimeRoutine());
        }

        if (click)
        {
            if (pickUp.IsHolding)
                pickUp.PlaceDown();
            else
                pickUp.PickUp();
        }
        
        if (moveDir.sqrMagnitude >= 0.1f)
        {
            var targetRotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cmCamera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(
                a: transform.rotation,
                b: Quaternion.Euler(0f, targetRotation, 0f),
                t: 1.0f - Mathf.Exp(-turnResponse * Time.deltaTime)
            );
            currentSpeed = Mathf.Lerp(
                a: currentSpeed,
                b: input.Sprint.IsPressed() ? sprintSpeed : moveSpeed,
                t: 1.0f - Mathf.Exp(-sprintResponse * Time.deltaTime)
            );
            Vector3 rotatedDir = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
            horizontalVelocity = rotatedDir * currentSpeed;
        }
        else
        {
            horizontalVelocity = Vector3.zero;
            currentSpeed = 0;
        }

        if (requestedJump)
        {
            if (IsGrounded || (!wasUngroundedByJump && timeSinceUngrounded < coyoteTime))
            {
                wasUngroundedByJump = true;
                verticalVelocity = jumpStrength;
            }
            else
            {
                timeSinceJumpRequest += Time.deltaTime;
                requestedJump = timeSinceJumpRequest < jumpBufferTime;
            }
        }
        
        if (IsGrounded)
        {
            timeSinceUngrounded = 0;
            wasUngroundedByJump = false;
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }
        }
        if (!IsGrounded)
        {
            
            verticalVelocity -= gravity * Time.deltaTime; // Might want to change this out for gravity inverting
            timeSinceUngrounded += Time.deltaTime;
            // Vector3 targetPlanarVelocity = Vector3.zero;
            // if (moveDir.sqrMagnitude > 0.1f)
            // {
            //     var targetRotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cmCamera.transform.eulerAngles.y;
            //     var targetMovement = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
            //     var movementForce = Time.deltaTime * airAcceleration * targetMovement;
            //     // targetPlanarVelocity = horizontalVelocity + movementForce;
            //     // targetPlanarVelocity = targetMovement * maxAirSpeed;
            //     horizontalVelocity = Vector3.RotateTowards(
            //         current: horizontalVelocity,
            //         target: targetMovement * horizontalVelocity.magnitude,
            //         maxRadiansDelta: airAcceleration * Time.deltaTime,
            //         maxMagnitudeDelta: 0f
            //     );
            // }
        }

        currentVelocity = horizontalVelocity;
        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxFallSpeed, maxFallSpeed);
        currentVelocity.y = verticalVelocity;
        controller.Move(currentVelocity * Time.deltaTime );
        animator.SetFloat("VelocityY", currentVelocity.y);
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("IsGrounded", IsGrounded);
    }

    private IEnumerator ToggleTimeRoutine()
    {
        animator.SetTrigger("ToggleTime");
        yield return new WaitForSeconds(0.2f);
        TimeManager.Instance.ToggleTime();
    }
}
