using System.Collections;
using UnityEngine;

public class CameraMainMenu : MonoBehaviour
{
    public Transform tableViewPoint; 
    public float zoomDuration = 2.0f; 

    public MonoBehaviour PickUp;

    private void Start()
    {
        if (PickUp != null)
        {
            PickUp.enabled = false;
        }
    }

    public void StartZoom()
    {
        StartCoroutine(ZoomToTable());
    }

    private IEnumerator ZoomToTable()
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 targetPos = tableViewPoint.position;
        Quaternion targetRot = Quaternion.Euler(70, 180, 0f);

        float timeElapsed = 0f;

        while (timeElapsed < zoomDuration)
        {
            float t = timeElapsed / zoomDuration;
            float smoothStep = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, targetPos, smoothStep);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, smoothStep);

            timeElapsed += Time.deltaTime;
            yield return null; 
        }

        transform.position = targetPos;
        transform.rotation = targetRot;


        if (PickUp != null)
        {
            PickUp.enabled = true;
        }
    }
}