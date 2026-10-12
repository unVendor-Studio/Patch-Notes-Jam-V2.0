using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartSceneButton;
    [SerializeField] private Button respawnButton;

    private bool isPaused = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(TogglePause);
        }

        if (restartSceneButton != null)
        {
            restartSceneButton.onClick.AddListener(RestartScene);
        }

        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(RespawnToCheckpoint);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !FindAnyObjectByType<PlayerRespawn>().isDying)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenuCanvas.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (MusicManager.Instance != null)
                MusicManager.Instance.PlayMenuButton();
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenuCanvas.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RespawnToCheckpoint()
    {
        TogglePause();

        PlayerRespawn playerRespawn = FindAnyObjectByType<PlayerRespawn>();
        if (playerRespawn != null)
        {
            playerRespawn.Die();
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}