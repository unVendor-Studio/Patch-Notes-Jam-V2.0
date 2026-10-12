using UnityEngine;

public class LightSourcePickup : MonoBehaviour
{
    [SerializeField] private GameObject playerLightAura; 
    [SerializeField] private string narrativeText = "ohhhhh u got a shard of the prism, the next room is highly unstable and there might be a lot of monster, looking at them will holding it can help with that";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.Instance.PlayNodePowerUp(transform.position);
            playerLightAura.SetActive(true);

                AINarrator.Instance.Speak(narrativeText);
        }
    }
}