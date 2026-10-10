using UnityEngine;

public class LightSourcePickup : MonoBehaviour
{
    [SerializeField] private GameObject playerLightAura; 
    [SerializeField] private string narrativeText = "The light source is strong, maybe it can deter something";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerLightAura.SetActive(true);

            if (AINarrator.Instance != null)
                AINarrator.Instance.Speak(narrativeText);

            Destroy(gameObject); 
        }
    }
}