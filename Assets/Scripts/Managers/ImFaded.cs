using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImFaded : MonoBehaviour
{
    public static ImFaded Instance { get; private set; }

    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private Image blackBox;
    [SerializeField] private float fadeDuration = 1.5f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (fadeCanvas != null && blackBox != null)
        {
            StartCoroutine(FadeInRoutine());
            GetComponent<PlayerRespawn>().isDying = false;
        }
    }

    public void FadeOutAndLoad(string sceneName)
    {
        if (fadeCanvas != null && blackBox != null)
        {
            StartCoroutine(FadeOutRoutine(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private IEnumerator FadeInRoutine()
    {
        GetComponent<PlayerRespawn>().isDying = false;

        fadeCanvas.SetActive(true);
        Color c = blackBox.color;
        c.a = 1f;
        blackBox.color = c;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            blackBox.color = c;
            yield return null;
        }
        fadeCanvas.SetActive(false);
    }

    private IEnumerator FadeOutRoutine(string sceneName)
    {
        GetComponent<PlayerRespawn>().isDying = false;

        fadeCanvas.SetActive(true);
        Color c = blackBox.color;
        c.a = 0f;
        blackBox.color = c;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            blackBox.color = c;
            yield return null;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(sceneName);
    }
}