using UnityEngine;

public class ElectricCharge : MonoBehaviour
{
    public PlayerInput player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !player.IsCharged)
        {
            player.IsCharged = true;
            Destroy(gameObject);
        }
    }
}
