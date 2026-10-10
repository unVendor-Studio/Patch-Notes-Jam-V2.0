using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]GameObject PlayButton;
    [SerializeField]GameObject ExitButton;

    [SerializeField] GameObject GamePlayCanvas;
    [SerializeField] GameObject text;

    void Start()
    {
        ExitButton.GetComponent<Button>().onClick.AddListener(() => ClickExit());
        PlayButton.GetComponent<Button>().onClick.AddListener(() => ClickPlay());

    }

    void Update()
    {
        
    }


    void ClickPlay()
    {
        GamePlayCanvas.SetActive(false);
        text.SetActive(true);
        FindFirstObjectByType<CameraMainMenu>().StartZoom();

    }
    void ClickExit()
    {
        Application.Quit();
    }

}
