using UnityEngine;

public class TimeReactivePlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool activeInFutureOnly = true;

    private bool canMove = true;

    private void OnEnable() => TimeManager.OnTimeSwapped += UpdateState;
    private void OnDisable() => TimeManager.OnTimeSwapped -= UpdateState;

    private void Start()
    {
        UpdateState(TimeManager.IsFuture);
    }

    private void UpdateState(bool isFuture)
    {
        canMove = activeInFutureOnly ? isFuture : !isFuture;
    }

    private void Update()
    {
        if (!canMove) return;

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}