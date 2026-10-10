using UnityEngine;

public class LevelPaper : MonoBehaviour
{
    [Header("Level Settings")]
    public string sceneToLoad;

    [Header("Animation Settings")]
    public float moveSpeed = 15f;
    public float hoverScaleMultiplier = 1.15f; 

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private Transform targetInspectPoint = null;
    private bool isBeingInspected = false;
    private bool isHovered = false;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
    }

    public void SetHovered(bool state)
    {
        isHovered = state;
    }

    public void Inspect(Transform inspectPoint)
    {
        targetInspectPoint = inspectPoint;
        isBeingInspected = true;
        isHovered = false; 
    }

    public void ReturnToTable()
    {
        isBeingInspected = false;
        targetInspectPoint = null;
    }

    void Update()
    {
        if (isBeingInspected && targetInspectPoint != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetInspectPoint.position, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation * Quaternion.Euler(10, 0 , 0), Time.deltaTime * moveSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * moveSpeed);

            Vector3 targetScale = isHovered ? originalScale * hoverScaleMultiplier : originalScale;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * moveSpeed);
        }
    }
}