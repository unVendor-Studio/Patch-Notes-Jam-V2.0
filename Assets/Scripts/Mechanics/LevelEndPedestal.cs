using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndPedestal : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string completionMessage = "Returning to ur house";

    [SerializeField] private float spinSpeed = 90f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.25f;

    private bool hasTriggered = false;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;

        if (TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);
        transform.position = startPosition + new Vector3(0f, Mathf.Sin(Time.time * bobSpeed) * bobHeight, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(EndLevelSequence(other.gameObject));
        }
    }

    private IEnumerator EndLevelSequence(GameObject player)
    {
        if (AINarrator.Instance != null) AINarrator.Instance.Speak(completionMessage);
        if (MusicManager.Instance != null) MusicManager.Instance.PlayLevelEnterWarp();

        yield return new WaitForSeconds(2f);

            ImFaded.Instance.FadeOutAndLoad(mainMenuSceneName);
    }
}