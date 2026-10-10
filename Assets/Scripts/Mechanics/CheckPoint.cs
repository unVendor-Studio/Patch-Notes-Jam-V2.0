using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            other.GetComponent<PlayerRespawn>().SetRespawnPoint(transform.position);

            Debug.Log("Checkpoint Saved!");
        }
    }
}