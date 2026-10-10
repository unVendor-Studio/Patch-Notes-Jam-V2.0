using UnityEngine;
using UnityEngine.Events;

public class LogicGate : MonoBehaviour
{
    [SerializeField] private int requiredPowerNodes = 1;
    private int currentPower = 0;

    public UnityEvent OnGateOpened;
    public UnityEvent OnGateClosed;

    public void AddPower()
    {
        currentPower++;
        if (currentPower >= requiredPowerNodes)
        {
            OnGateOpened?.Invoke(); 
        }
    }

    public void RemovePower()
    {
        bool wasOpen = currentPower >= requiredPowerNodes;
        currentPower--;

        if (wasOpen && currentPower < requiredPowerNodes)
        {
            OnGateClosed?.Invoke();
        }
    }
}