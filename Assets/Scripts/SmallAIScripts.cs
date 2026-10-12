using System.Collections;
using UnityEngine;

public class SmallAIScripts : MonoBehaviour
{
    private AINarrator narrator;

    public string messages;
    public bool talked = false;
    void Start()
    {
        narrator = AINarrator.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!talked)
        {
            narrator.Speak(messages);
            StartCoroutine(startTalking());
        }
    }

    IEnumerator startTalking()
    {
        talked = true;
        yield return new WaitForSeconds(5);
        talked = false;
    }
}
