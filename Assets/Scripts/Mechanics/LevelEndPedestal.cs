using System.Collections;
using UnityEngine;

public class LevelEndPedestal : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string completionMessage = "Returning to ur house";

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
        if (player.TryGetComponent(out CharacterController cc))
        {
            cc.enabled = false;
        }
        if (player.TryGetComponent(out PlayerInput input))
        {
            input.enabled = false;
        }

        if (AINarrator.Instance != null) AINarrator.Instance.Speak(completionMessage);
        if (MusicManager.Instance != null) MusicManager.Instance.PlayLevelEnterWarp();

        yield return new WaitForSeconds(3.5f);

        if (ImFaded.Instance != null)
        {
            ImFaded.Instance.FadeOutAndLoad(mainMenuSceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}