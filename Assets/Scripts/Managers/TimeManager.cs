using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public static event Action<bool> OnTimeSwapped;

    public static bool IsFuture { get; private set; } = true;

    private List<Light> futurePointLights = new List<Light>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Light[] allLights = FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Light light in allLights)
        {
            if (light.type == LightType.Point)
            {
                futurePointLights.Add(light);
            }
        }
    }

    private void Start()
    {
        ApplyTimeState();

    }

    public void ToggleTime()
    {
        IsFuture = !IsFuture;
        ApplyTimeState();
        OnTimeSwapped?.Invoke(IsFuture);
    }

    private void ApplyTimeState()
    {
        if (Camera.main != null)
        {
            Camera.main.cullingMask = IsFuture ?
                LayerMask.GetMask("Default", "Player", "UI", "Future") :
                LayerMask.GetMask("Default", "Player", "UI", "Past");
        }

        foreach (Light light in futurePointLights)
        {
            if (light != null)
            {
                light.enabled = IsFuture;
            }
        }

        int playerLayer = LayerMask.NameToLayer("Player");
        int futureLayer = LayerMask.NameToLayer("Future");
        int pastLayer = LayerMask.NameToLayer("Past");

        Physics.IgnoreLayerCollision(playerLayer, pastLayer, IsFuture);
        Physics.IgnoreLayerCollision(playerLayer, futureLayer, !IsFuture);
    }
}