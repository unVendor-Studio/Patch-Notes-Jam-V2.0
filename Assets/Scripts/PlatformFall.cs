using System.Collections;
using UnityEngine;

public class PlatformFall : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private float shakeDuration = 0.8f;
    [SerializeField] private float shakeIntensity = 0.5f;
    [SerializeField] private float dropDistance = 20f;
    [SerializeField] private float dropSpeed = 25f;
    [SerializeField] private float respawnTime = 2f;

    private Vector3 startPos;
    private bool isTriggered = false;
    private Collider col;
    private MeshRenderer mesh;

    private void Start()
    {
        startPos = transform.position;
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            StartCoroutine(FallSequence());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isTriggered)
        {
            StartCoroutine(FallSequence());
        }
    }

    private IEnumerator FallSequence()
    {
        isTriggered = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position = startPos + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Vector3 dropTarget = startPos - new Vector3(0, dropDistance, 0);
        col.enabled = false;

        while (transform.position.y > dropTarget.y)
        {
            transform.position += Vector3.down * (dropSpeed * Time.deltaTime);
            yield return null;
        }

        mesh.enabled = false;
        yield return new WaitForSeconds(respawnTime);

        transform.position = startPos;
        mesh.enabled = true;
        col.enabled = true;
        isTriggered = false;
    }
}