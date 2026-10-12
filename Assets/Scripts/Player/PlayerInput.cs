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

    public bool IsCharged;

    private InputSystem inputSystem;
    private PickUpItems pickUp;
    private PlayerPhysicsModifier physicsMod;
    private float currentSpeed;
    private Vector3 currentVelocity;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    private bool wasUngroundedByJump;
    private bool wasAirborne;
    private float timeSinceUngrounded;
    private float timeSinceJumpRequest;
    private bool requestedJump = false;
    [SerializeField] private CinemachineInputAxisController inputController;
    private float defaultXMultiplier;
    private float defaultYMultiplier;
    public bool IsGrounded
    {
        get
        {
            if (physicsMod == null || physicsMod.GravityDirection == 1)
                return controller.isGrounded;
            return (controller.collisionFlags & CollisionFlags.Above) != 0;
        }
    }
    private void Awake()
    {
        inputSystem = new InputSystem();
        inputSystem.Enable();
        if (inputController != null && inputController.Controllers.Count >= 2)
        {
            defaultXMultiplier = inputController.Controllers[0].Input.Gain;
            defaultYMultiplier = inputController.Controllers[1].Input.Gain;
        }
    }
    private void Start()
    {
        requestedJump = false;
        Cursor.lockState = CursorLockMode.Locked;
        pickUp = GetComponent<PickUpItems>();
        physicsMod = GetComponent<PlayerPhysicsModifier>();
        var cmInput = cmCamera.GetComponent<CinemachineInputAxisController>();
    }
    private void Update()
    {
        animator.SetBool("WasAirborne", wasAirborne);
        var input = inputSystem.Player;
        var movement = input.Move.ReadValue<Vector2>();
        var jump = input.Jump.WasPressedThisFrame();
        var click = input.Attack.WasPressedThisFrame();
        var interact = input.Interact.WasPressedThisFrame();
        bool wasRequestedJump = requestedJump;
        requestedJump = requestedJump || jump;
        if (requestedJump && !wasRequestedJump)
        {
            timeSinceJumpRequest = 0;
        }
        int gravDir = physicsMod != null ? physicsMod.GravityDirection : 1;

        Vector3 moveDir = new Vector3(movement.x * gravDir, 0, movement.y);
        moveDir = Vector3.ClampMagnitude(moveDir, 1);

        if (interact) StartCoroutine(ToggleTimeRoutine());
        if (click)
        {
            if (pickUp.IsHolding) pickUp.PlaceDown();
            else pickUp.PickUp();
        }
        bool isIce = physicsMod != null && physicsMod.IsIceFriction;
        float currentFriction = isIce ? 1.5f : sprintResponse;
        if (moveDir.sqrMagnitude >= 0.1f)
        {
            var targetRotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cmCamera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(
            a: transform.rotation,
            b: Quaternion.Euler(0f, targetRotation, 0f),
            t: 1.0f - Mathf.Exp(-turnResponse * Time.deltaTime)
            );
            float targetSpeed = input.Sprint.IsPressed() ? sprintSpeed : moveSpeed;
            Vector3 rotatedDir = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, rotatedDir * targetSpeed, 1.0f - Mathf.Exp(-currentFriction * Time.deltaTime));
            currentSpeed = horizontalVelocity.magnitude;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, 1.0f - Mathf.Exp(-currentFriction * Time.deltaTime));
            currentSpeed = horizontalVelocity.magnitude;
        }
        if (requestedJump)
        {
            if (IsGrounded || (!wasUngroundedByJump && timeSinceUngrounded < coyoteTime))
            {
                wasUngroundedByJump = true;
                verticalVelocity = jumpStrength * gravDir;
                MusicManager.Instance.PlayJumpSound(transform.position);
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
            if (gravDir == 1 && verticalVelocity < 0f) verticalVelocity = -2f;
            else if (gravDir == -1 && verticalVelocity > 0f) verticalVelocity = 2f;
            wasAirborne = false;
        }
        if (!IsGrounded)
        {
            verticalVelocity -= (gravity * gravDir) * Time.deltaTime;
            timeSinceUngrounded += Time.deltaTime;
            wasAirborne = true;
        }
        currentVelocity = horizontalVelocity;
        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxFallSpeed, maxFallSpeed);
        currentVelocity.y = verticalVelocity;
        controller.Move(currentVelocity * Time.deltaTime);
        animator.SetFloat("VelocityY", currentVelocity.y);
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("IsGrounded", IsGrounded);
    }
    private IEnumerator ToggleTimeRoutine()
    {
        animator.SetTrigger("ToggleTime");
        MusicManager.Instance.PlayDeviceClick(transform.position);
        yield return new WaitForSeconds(0.2f);
        TimeManager.Instance.ToggleTime();
        MusicManager.Instance.PlaySwapToPast(transform.position);
    }
}