using UnityEngine;

public class PrismMonster : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float chaseSpeed = 4.5f;
    [SerializeField] private float retreatSpeed = 8f;
    [SerializeField] private bool isFlyingEnemy = true;


    [SerializeField] private bool onlyActiveInPast = true;

    [SerializeField] private float stareTimeToKill = 0.5f;

    [SerializeField] private float lookThreshold = 0.75f;

    [SerializeField] private float sightRange = 30f;

    private Transform player;
    private Camera mainCam;
    private bool isActiveInCurrentTimeline = false;
    private bool isFleeing = false;

    private float currentStareTime = 0f;

    private void Start()
    {
        mainCam = Camera.main;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            SpawnBehindPlayer();
        }

        UpdateTimelineState(TimeManager.IsFuture);
    }

    private void SpawnBehindPlayer()
    {
        float randomDistance = Random.Range(10f, 15f);

        Vector3 spawnDir = -mainCam.transform.forward;

        if (!isFlyingEnemy) spawnDir.y = 0;

        Vector3 spawnPos = player.position + (spawnDir.normalized * randomDistance);
        if (!isFlyingEnemy) spawnPos.y = transform.position.y;

        transform.position = spawnPos;
    }

    private void OnEnable() => TimeManager.OnTimeSwapped += UpdateTimelineState;
    private void OnDisable() => TimeManager.OnTimeSwapped -= UpdateTimelineState;

    private void UpdateTimelineState(bool isFuture)
    {
        isActiveInCurrentTimeline = onlyActiveInPast ? !isFuture : isFuture;

        if (isActiveInCurrentTimeline && !isFleeing)
        {
            if (MusicManager.Instance != null)
                MusicManager.Instance.PlayMonsterRush(transform.position);
        }
    }

    private void Update()
    {
        if (!isActiveInCurrentTimeline || player == null) return;

        if (isFleeing)
        {
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            transform.position += fleeDirection * retreatSpeed * Time.deltaTime;
            return;
        }

        Vector3 targetPos = player.position;
        if (!isFlyingEnemy) targetPos.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
        transform.LookAt(targetPos);

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

        if (lookDot >= lookThreshold)
        {
            if (Physics.Raycast(mainCam.transform.position, directionToMonster, out RaycastHit hit, sightRange))
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