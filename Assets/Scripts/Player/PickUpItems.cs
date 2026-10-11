using System;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItems : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image aabbBox;
    [SerializeField] private Transform target;
    [SerializeField] private Transform dropTarget;
    
    [Header("Pick up settings")]
    [SerializeField] private float pickUpRadius;

    public bool IsHolding;
    private Collider closestObject;
    private void Update()
    {
        if (!IsHolding)
        {
            var colliders = Physics.OverlapSphere(transform.position, pickUpRadius, LayerMask.GetMask("PickUp"));
            if (colliders.Length == 0)
            {
                ToggleHighlight(false);
                closestObject = null;
                return;
            }
            float closestObjectToPlayer = float.MaxValue;
            foreach (var col in colliders)
            {
                
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < closestObjectToPlayer)
                {
                    closestObjectToPlayer = Vector3.Distance(transform.position, col.transform.position);
                    closestObject = col;
                }
            }
            UpdateHighlight(closestObject);
        }
        else
        {
            
        }
    }

    private void UpdateHighlight(Collider col)
    {
        if (col.TryGetComponent<Renderer>(out var renderr))
        {
            ToggleHighlight(true);

            Bounds aabb = renderr.bounds;
            Vector3 min = aabb.min;
            Vector3 max = aabb.max;

            Vector2 maxMax = new Vector2(float.MinValue, float.MinValue);
            Vector2 minMin = new Vector2(float.MaxValue, float.MaxValue);
            for (int i = 0; i < 8; ++i)
            {
                Vector3 pos = new Vector3((i % 2 == 0) ? min.x : max.x,
                    (i == 0 || i == 1 || i == 4 || i == 5) ? min.y : max.y, (i > 3) ? max.z : min.z);
                Vector3 viewPos = mainCamera.WorldToScreenPoint(pos);
                if (viewPos.x > maxMax.x)
                    maxMax.x = viewPos.x;
                if (viewPos.y > maxMax.y)
                    maxMax.y = viewPos.y;
                if (viewPos.x < minMin.x)
                    minMin.x = viewPos.x;
                if (viewPos.y < minMin.y)
                    minMin.y = viewPos.y;
            }

            float width = maxMax.x - minMin.x;
            float height = maxMax.y - minMin.y;
            Vector2 center = (maxMax + minMin) * 0.5f;

            aabbBox.rectTransform.sizeDelta = new Vector2(width, height);
            aabbBox.rectTransform.position = center;

        }
    }

    public void PickUp()
    {
        if (IsHolding || closestObject == null) return;
        ToggleHighlight(false);
        IsHolding = true;
        closestObject.transform.SetParent(this.transform);
        Vector3 localTarget = this.transform.InverseTransformPoint(target.position);
        Tween.LocalPosition(closestObject.transform, localTarget, duration: 0.2f);
        
    }

    public void PlaceDown()
    {
        if (!IsHolding) return;
        IsHolding = false;
        closestObject.transform.SetParent(null);
        Tween.Position(closestObject.transform, dropTarget.position, duration: 0.2f);
        
    }

    private void ToggleHighlight(bool activate)
    {
        if (activate)
        {
            if (aabbBox.TryGetComponent<HighlightAABB>(out var aabbbox))
                aabbbox.Highlight();
        }
        else
        {
            if (aabbBox.TryGetComponent<HighlightAABB>(out var aabb))
                aabb.Unhighlight();
        }
    }
}
