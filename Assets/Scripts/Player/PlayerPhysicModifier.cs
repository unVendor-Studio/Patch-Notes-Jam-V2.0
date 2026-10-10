using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerPhysicsModifier : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    [Header("Level 2: Gravity (Past Inverts)")]
    public bool levelHasGravityGimmick = false;
    private float defaultGravityY = -9.81f;

    [Header("Level 5: Friction (Past is Ice)")]
    public bool levelHasFrictionGimmick = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnEnable() => TimeManager.OnTimeSwapped += HandlePhysicsChange;
    private void OnDisable() => TimeManager.OnTimeSwapped -= HandlePhysicsChange;

    private void HandlePhysicsChange(bool isFuture)
    {
        if (levelHasGravityGimmick)
        {
            Physics.gravity = isFuture ? new Vector3(0, defaultGravityY, 0) : new Vector3(0, -defaultGravityY, 0);

            transform.localScale = isFuture ? Vector3.one : new Vector3(1, -1, 1);
        }
    }
}