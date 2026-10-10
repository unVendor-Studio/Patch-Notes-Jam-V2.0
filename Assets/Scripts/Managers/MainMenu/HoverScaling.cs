using UnityEngine;
using UnityEngine.EventSystems; // Required for UI hovering

public class HoverScaling : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    public float scaleMultiplier = 1.15f;
    public float lerpSpeed = 15f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * lerpSpeed);
    }

    // Triggers when the mouse enters the UI graphic
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleMultiplier;
    }

    // Triggers when the mouse leaves the UI graphic
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}