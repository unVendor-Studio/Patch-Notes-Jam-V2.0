using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ghostPref;
    GameObject player;

    private bool notspawned = true;
    void Start()
    {
        player = FindFirstObjectByType<PlayerInput>().gameObject;
    }

    private void Update()
    {
        if (player.transform.position.z >= 27.5 && notspawned)
        {
            InvokeRepeating("SummoningGhost", 100f, 3f);
            notspawned = false;
        }
        if (player.transform.position.z >= 51.8)
        {
            Destroy(this.gameObject);
        }
    }
    public void SummoningGhost()
    {
        Instantiate(ghostPref);
    }

    
}
