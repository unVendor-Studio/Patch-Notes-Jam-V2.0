using UnityEngine;

public class GravityVortex : MonoBehaviour
{
    [SerializeField] private float suckForce = 40f;
    [SerializeField] private bool onlyActiveInPast = true;

    private void OnTriggerStay(Collider other)
    {
        if (onlyActiveInPast && TimeManager.IsFuture) return;

        if (other.TryGetComponent(out Rigidbody rb))
        {
            Vector3 pullDirection = (transform.position - other.transform.position).normalized;

            rb.AddForce(pullDirection * suckForce, ForceMode.Acceleration);
        }
    }
}