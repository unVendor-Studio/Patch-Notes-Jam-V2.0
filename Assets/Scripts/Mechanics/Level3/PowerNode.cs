using UnityEngine;

public class PowerNode : MonoBehaviour
{
    [SerializeField] private LogicGate targetGate;
    private bool isPowered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isPowered && other.CompareTag("Player"))
        {
            isPowered = true;
            targetGate.AddPower();
            MusicManager.Instance.PlayNodePowerUp(transform.position);
        }
    }

    public void LosePower()
    {
        if (isPowered)
        {
            isPowered = false;
            targetGate.RemovePower();
        }
    }
}