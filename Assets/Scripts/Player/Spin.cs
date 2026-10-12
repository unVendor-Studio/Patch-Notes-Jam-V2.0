using UnityEngine;

public class Spin : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Degrees per second around X, Y, and Z axes.")]
    [SerializeField] private Vector3 spinSpeed = new Vector3(0f, 50f, 0f);
    
    [Tooltip("Rotate relative to local transform axes or global world space.")]
    [SerializeField] private Space rotationSpace = Space.Self;

    [Header("Floating Settings")]
    [Tooltip("Maximum height distance moved above and below starting position.")]
    [SerializeField] private float amplitude = 0.5f;

    [Tooltip("Speed/frequency of the vertical bobbing cycle.")]
    [SerializeField] private float frequency = 2f;

    [Header("Options")]
    [Tooltip("Keep animating while the game is paused (Time.timeScale = 0).")]
    [SerializeField] private bool useUnscaledTime = false;

    private Vector3 startPosition;

    private void Start()
    {
        // Store starting position relative to parent or world origin
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        float currentTime = useUnscaledTime ? Time.unscaledTime : Time.time;

        // 1. Continuous Rotation
        transform.Rotate(spinSpeed * deltaTime, rotationSpace);

        // 2. Smooth Sine-Wave Bobbing
        float newY = startPosition.y + (Mathf.Sin(currentTime * frequency) * amplitude);
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
