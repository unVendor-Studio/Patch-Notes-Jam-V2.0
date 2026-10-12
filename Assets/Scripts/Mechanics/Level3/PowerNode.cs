using Unity.VisualScripting;
using UnityEngine;

public class PowerNode : MonoBehaviour
{
    [SerializeField] private LogicGate targetGate;
    [SerializeField] private GameObject onState;
    [SerializeField] private GameObject offState;
    public bool IsPressurePlate;
    private bool isPowered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isPowered && other.CompareTag("Player"))
        {
            isPowered = true;
            targetGate.AddPower();
            MusicManager.Instance.PlayNodePowerUp(transform.position);
            if (offState != null && onState != null)
            {
                offState.SetActive(false);
                onState.SetActive(true);
            }
            
        }
    }

    public void OnTriggerExit()
    {
        if (isPowered && IsPressurePlate)
        {
            isPowered = false;
            targetGate.RemovePower();
            if (offState != null && onState != null)
            {
                offState.SetActive(true);
                onState.SetActive(false);
            }
            
        }
    }
}