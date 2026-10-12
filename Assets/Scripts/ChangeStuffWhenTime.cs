using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private Material futureSkybox;
    [SerializeField] private Material pastSkybox;

    [SerializeField] private Material futureMaterial;
    [SerializeField] private Material pastMaterial;
    [SerializeField] private string targetTag = "Change";

    private GameObject[] changingObjects;

    private void Start()
    {
        changingObjects = GameObject.FindGameObjectsWithTag(targetTag);

        UpdateEnvironment(TimeManager.IsFuture);
    }

    private void OnEnable() => TimeManager.OnTimeSwapped += UpdateEnvironment;
    private void OnDisable() => TimeManager.OnTimeSwapped -= UpdateEnvironment;

    private void UpdateEnvironment(bool isFuture)
    {
        if (futureSkybox != null && pastSkybox != null)
        {
            RenderSettings.skybox = isFuture ? futureSkybox : pastSkybox;
        }

        Material targetMaterial = isFuture ? futureMaterial : pastMaterial;

        foreach (GameObject obj in changingObjects)
        {
            if (obj != null && obj.TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer.material = targetMaterial;
            }
        }
    }
}