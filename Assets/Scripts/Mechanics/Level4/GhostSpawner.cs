using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ghostPref;
    [Header("Event Settings")]
    [SerializeField] private GameObject eventLight; 

    public GameObject player;
    private Camera mainCam;

    private bool isSpawning = false;

    void Start()
    {
        player = FindFirstObjectByType<PlayerInput>().gameObject;
        mainCam = Camera.main;

        if (eventLight != null) eventLight.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        float playerZ = player.transform.position.z;

        if (playerZ >= 27.5f && playerZ < 51.8f)
        {
            if (!isSpawning)
            {
                isSpawning = true;
                if (eventLight != null) eventLight.SetActive(true);
                InvokeRepeating(nameof(SummoningGhost), 0f, 3f);
            }
        }
        else if (isSpawning && (playerZ < 27.5f || playerZ >= 51.8f))
        {
            ResetEvent();
        }
    }

    public void ResetEvent()
    {
        isSpawning = false;
        CancelInvoke(nameof(SummoningGhost));

        if (eventLight != null) eventLight.SetActive(false);

        PrismMonster[] activeGhosts = FindObjectsByType<PrismMonster>(FindObjectsSortMode.None);
        foreach (PrismMonster ghost in activeGhosts)
        {
            if (ghost != null) Destroy(ghost.gameObject);
        }
    }

    public void SummoningGhost()
    {
        if (player == null || mainCam == null) return;

        float randomDistance = Random.Range(10f, 15f);
        Vector3 spawnDir = -mainCam.transform.forward;
        Vector3 spawnPos = player.transform.position + (spawnDir * randomDistance);

        Instantiate(ghostPref, spawnPos, Quaternion.identity);
    }
}