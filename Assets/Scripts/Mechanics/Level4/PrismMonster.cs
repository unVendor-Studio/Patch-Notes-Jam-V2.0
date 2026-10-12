using UnityEngine;

public class PrismMonster : MonoBehaviour
{
    [Header("Movement")]
    private float chaseSpeed = 2f;
    private float retreatSpeed = 20f;

    [SerializeField] private bool onlyActiveInPast = true;
    [SerializeField] private float stareTimeToKill = 0.5f;

    [Header("Vision Settings")]
    [Tooltip("Lower this number (e.g., 0.3) to make the vision cone wider. 1 = pinpoint accuracy.")]
    [SerializeField] private float lookThreshold = 0.5f;
    [SerializeField] private float sightRange = 30f;

    private Transform player;
    private Camera mainCam;
    private bool isActiveInCurrentTimeline = false;
    private bool isFleeing = false;
    private float currentStareTime = 0f;

    private Collider col;
    private Renderer[] meshes;

    private void Start()
    {
        mainCam = Camera.main;
        col = GetComponent<Collider>();
        meshes = GetComponentsInChildren<Renderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        UpdateTimelineState(TimeManager.Instance.IsFuture);
    }

    private void OnEnable() => TimeManager.OnTimeSwapped += UpdateTimelineState;
    private void OnDisable() => TimeManager.OnTimeSwapped -= UpdateTimelineState;

    private void UpdateTimelineState(bool isFuture)
    {
        isActiveInCurrentTimeline = onlyActiveInPast ? !isFuture : isFuture;

        if (col != null) col.enabled = isActiveInCurrentTimeline;
        foreach (var mesh in meshes)
        {
            mesh.enabled = isActiveInCurrentTimeline;
        }

        if (isActiveInCurrentTimeline && !isFleeing)
        {
            if (MusicManager.Instance != null)
                MusicManager.Instance.PlayMonsterRush(transform.position);
        }
    }

    private void Update()
    {
        if (!isActiveInCurrentTimeline || player == null) return;

        transform.LookAt(player.position);

        if (isFleeing)
        {
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            transform.position += fleeDirection * retreatSpeed * Time.deltaTime;
            return;
        }

        Vector3 targetPos = player.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);

        CheckPlayerSight();
    }

    private void CheckPlayerSight()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, mainCam.transform.position);

        if (distanceToPlayer > sightRange)
        {
            currentStareTime = 0f;
            return;
        }

        Vector3 directionToMonster = (transform.position - mainCam.transform.position).normalized;
        float lookDot = Vector3.Dot(mainCam.transform.forward, directionToMonster);

        // This checks if the monster falls within your new wider vision cone
        if (lookDot >= lookThreshold)
        {
            int layerMask = ~LayerMask.GetMask("Player");

            if (Physics.Raycast(mainCam.transform.position, directionToMonster, out RaycastHit hit, sightRange, layerMask))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    currentStareTime += Time.deltaTime;

                    if (currentStareTime >= stareTimeToKill)
                    {
                        Die();
                    }
                    return;
                }
            }
        }

        currentStareTime = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFleeing || !isActiveInCurrentTimeline) return;

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerRespawn playerRespawn))
            {
                playerRespawn.Die();
            }
        }
    }

    private void Die()
    {
        isFleeing = true;
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayMonsterRetreat(transform.position);

        Destroy(gameObject, 2f);
    }
}