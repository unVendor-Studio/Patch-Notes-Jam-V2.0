using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]GameObject PlayButton;
    [SerializeField]GameObject ExitButton;
    [SerializeField] GameObject CreditButton;
    [SerializeField] GameObject BackButton;


    [SerializeField] GameObject GamePlayCanvas;
    [SerializeField] GameObject GamePlayPanel;
    [SerializeField] GameObject CreditCanvas;
    [SerializeField] GameObject text;

    void Start()
    {
        ExitButton.GetComponent<Button>().onClick.AddListener(() => ClickExit());
        PlayButton.GetComponent<Button>().onClick.AddListener(() => ClickPlay());
        CreditButton.GetComponent<Button>().onClick.AddListener(() => clickCredit());
        PlayButton.GetComponent<Button>().onClick.AddListener(() => clickBack());

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

    void clickCredit()
    {
        GamePlayPanel.SetActive(false);
        CreditCanvas.SetActive(true);
    }
    public void clickBack()
    {
        GamePlayPanel.SetActive(true);
        CreditCanvas.SetActive(false);
    }

}
