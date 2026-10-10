using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndPedestal : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string completionMessage = "Anomaly secured. Returning to Hub.";

    private bool hasTriggered = false;

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
        if (player.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        if (AINarrator.Instance != null) AINarrator.Instance.Speak(completionMessage);
        if (MusicManager.Instance != null) MusicManager.Instance.PlayLevelEnterWarp(); 

        yield return new WaitForSeconds(3.5f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}