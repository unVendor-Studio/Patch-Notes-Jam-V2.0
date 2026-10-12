using UnityEngine;

public class CubeUnlocker : MonoBehaviour
{
    public void AddUnlock()
    {
        DoorUnlock.Instance.UnlockCount++;
    }
}
