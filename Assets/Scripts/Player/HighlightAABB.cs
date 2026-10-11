using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class HighlightAABB : MonoBehaviour
{
    public float response = 10f;
    public float initialScale;
    public float targetScale;

    private RectTransform rectTransform;
    private Image image;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(initialScale, initialScale, initialScale);
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void Highlight()
    {
        if (!image.enabled)
        {
            image.enabled = true;
            
            rectTransform.localScale = new Vector3(initialScale, initialScale, initialScale);
            Tween.Scale(rectTransform, new Vector3(targetScale, targetScale, targetScale), duration: 0.5f);
        }
        
    }

    public void Unhighlight()
    {
        image.enabled = false;
    }

}
