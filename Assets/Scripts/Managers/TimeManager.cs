using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Global static event: true = Future, false = Past
    public static event Action<bool> OnTimeSwapped;

    public static bool IsFuture { get; private set; } = true;

    [Header("Layer Setup")]
    [SerializeField] private string futureLayerName = "Future";
    [SerializeField] private string pastLayerName = "Past";
    [SerializeField] private string playerLayerName = "Player";

    private int futureLayer;
    private int pastLayer;
    private int playerLayer;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        futureLayer = LayerMask.NameToLayer(futureLayerName);
        pastLayer = LayerMask.NameToLayer(pastLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);

        ApplyTimeState(IsFuture);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            IsFuture = !IsFuture;
            ApplyTimeState(IsFuture);

            OnTimeSwapped?.Invoke(IsFuture);
        }
    }

    private void ApplyTimeState(bool futureActive)
    {
        if (futureActive)
        {
            mainCam.cullingMask |= (1 << futureLayer);
            mainCam.cullingMask &= ~(1 << pastLayer);

            Physics.IgnoreLayerCollision(playerLayer, pastLayer, true);
            Physics.IgnoreLayerCollision(playerLayer, futureLayer, false);
        }
        else
        {
            mainCam.cullingMask |= (1 << pastLayer);
            mainCam.cullingMask &= ~(1 << futureLayer);

            Physics.IgnoreLayerCollision(playerLayer, futureLayer, true);
            Physics.IgnoreLayerCollision(playerLayer, pastLayer, false);
        }
    }
}