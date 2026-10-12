using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [Header("Target & Materials")]
    [Tooltip("The Renderer component. If left empty, automatically finds the Renderer on this object.")]
    [SerializeField] private Renderer targetRenderer;

    [Tooltip("The replacement material to assign when triggered.")]
    [SerializeField] private Material targetMaterial;

    private Material originalMaterial;
    private bool isToggled = false;

    private void Awake()
    {
        // Automatically fetch MeshRenderer if not manually assigned
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        // Cache the starting material
        if (targetRenderer != null)
        {
            originalMaterial = targetRenderer.sharedMaterial;
        }
    }

    /// <summary>
    /// Swaps the current material to 'targetMaterial'.
    /// </summary>
    public void ApplyTargetMaterial()
    {
        if (targetRenderer != null && targetMaterial != null)
        {
            targetRenderer.material = targetMaterial;
        }
    }

    /// <summary>
    /// Resets the cube back to its initial starting material.
    /// </summary>
    public void ResetMaterial()
    {
        if (targetRenderer != null && originalMaterial != null)
        {
            targetRenderer.material = originalMaterial;
        }
    }

    /// <summary>
    /// Toggles back and forth between the original and target material.
    /// </summary>
    public void ToggleMaterial()
    {
        if (isToggled)
        {
            ResetMaterial();
        }
        else
        {
            ApplyTargetMaterial();
        }
        isToggled = !isToggled;
    }

    /// <summary>
    /// Assigns any custom material passed dynamically from code.
    /// </summary>
    public void SetCustomMaterial(Material customMaterial)
    {
        if (targetRenderer != null && customMaterial != null)
        {
            targetRenderer.material = customMaterial;
        }
    }
}