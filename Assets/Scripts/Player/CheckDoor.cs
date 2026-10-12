using System;
using UnityEngine;

public class CheckDoor : MonoBehaviour
{
    [SerializeField] private float doorCheckRadius = 3f;
    private SphereCollider sphereCollider;
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Door>(out var door))
        {
            door.Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Door>(out var door))
        {
            door.Close();
        }
    }
}
