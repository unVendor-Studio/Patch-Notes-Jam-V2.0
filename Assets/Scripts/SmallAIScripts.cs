using UnityEngine;

public class SmallAIScripts : MonoBehaviour
{
    private AINarrator narrator;

    public string messages;
    void Start()
    {
        narrator = AINarrator.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        narrator.Speak(messages);
    }
}
