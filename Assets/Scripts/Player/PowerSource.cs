using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public PlayerInput player;
    private MaterialChanger mater;
    void Start()
    {
        mater = GetComponent<MaterialChanger>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player.IsCharged)
        {
            player.IsCharged = false;
            mater.ApplyTargetMaterial();
        }
    }
}
