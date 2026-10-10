using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRespawn : MonoBehaviour
{
    private Vector3 currentRespawnPosition;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentRespawnPosition = transform.position;
    }

    public void SetRespawnPoint(Vector3 newPosition)
    {
        currentRespawnPosition = newPosition;
    }

    public void Die()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = currentRespawnPosition;

        MusicManager.Instance.PlayJumpSound(transform.position);
    }
}