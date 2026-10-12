using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerRespawn : MonoBehaviour
{
    private Vector3 currentRespawnPosition;
    private CharacterController cc;

    public bool isDying = false;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        currentRespawnPosition = transform.position;
    }

    public void SetRespawnPoint(Vector3 newPosition)
    {
        currentRespawnPosition = newPosition;
    }

    public void Die()
    {
        cc.enabled = false;
        transform.position = currentRespawnPosition;
        cc.enabled = true;

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayJumpSound(transform.position);

        if (TimeManager.Instance != null)
            TimeManager.Instance.DieCanvas.SetActive(true);

        isDying = true;
    }
}