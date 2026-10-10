using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUp : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public Transform inspectPoint;

    [Header("Settings")]
    public float interactDistance = 10f;

    private LevelPaper currentlyHeldPaper = null;
    private LevelPaper currentlyHoveredPaper = null;

    void Update()
    {
        if (currentlyHeldPaper == null)
        {
            HandleHoverAndPickup();
        }
        else
        {
            HandleHeldPaper();
        }
    }

    private void HandleHoverAndPickup()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            LevelPaper paper = hit.collider.GetComponent<LevelPaper>();

            if (paper != null)
            {
                if (currentlyHoveredPaper != paper)
                {
                    if (currentlyHoveredPaper != null) currentlyHoveredPaper.SetHovered(false);
                    currentlyHoveredPaper = paper;
                    currentlyHoveredPaper.SetHovered(true);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    currentlyHoveredPaper.SetHovered(false);
                    currentlyHoveredPaper = null;

                    currentlyHeldPaper = paper;
                    currentlyHeldPaper.Inspect(inspectPoint);
                }

                return; 
            }
        }

        if (currentlyHoveredPaper != null)
        {
            currentlyHoveredPaper.SetHovered(false);
            currentlyHoveredPaper = null;
        }
    }

    private void HandleHeldPaper()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene(currentlyHeldPaper.sceneToLoad);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            currentlyHeldPaper.ReturnToTable();
            currentlyHeldPaper = null;
        }
    }
}