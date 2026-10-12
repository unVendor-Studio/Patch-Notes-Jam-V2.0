using System.Collections;
using UnityEngine;

public class PlatformBob : MonoBehaviour
{
    [Header("Bob Settings")]
    [SerializeField] private float bobDistance = 0.5f;
    [SerializeField] private float bobSpeed = 15f;
    [SerializeField] private float returnSpeed = 5f;

    private Vector3 startPos;
    public bool isBobbing = false;

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!isBobbing)
        {
            StartCoroutine(BobSequence());
        }
    }
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (!isBobbing)
    //    {
    //        StartCoroutine(BobSequence());
    //    }
    //}
    private IEnumerator BobSequence()
    {
        MusicManager.Instance.PlayCubeThud(transform.position);
        isBobbing = true;
        Vector3 targetPos = startPos - new Vector3(0, bobDistance, 0);

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * bobSpeed);
            yield return null;
        }

        while (Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.position = startPos;
        isBobbing = false;
    }
}