using PrimeTween;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float blinkInterval = 0.5f;
    [SerializeField] private PickUpItems pickUp;
    [SerializeField] private Transform dropTarget;

    private Material materialInstance;
    private float timer;
    private bool isOn = true;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();
        targetRenderer.enabled = false;
        // Accessing .material creates a local instance copy so other objects using this material don't blink
    }

    void Update()
    {
        Tween.Position(transform, dropTarget.position, duration: 0.4f);
        if (!pickUp.IsHolding)
        {
            targetRenderer.enabled = false;
            return;
        }
        timer += Time.deltaTime;
        if (timer >= blinkInterval)
        {
            timer = 0f;
            isOn = !isOn;
            targetRenderer.enabled = isOn;
        }
    }
}
