using System.Collections;
using UnityEngine;
using TMPro;

public class AINarrator : MonoBehaviour
{
    public static AINarrator Instance;

    [SerializeField] private TextMeshProUGUI narratorText;
    [SerializeField] private float typingSpeed = 0.05f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        narratorText.text = "";
    }

    public void Speak(string message)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(TypewriterEffect(message));
    }

    private IEnumerator TypewriterEffect(string message)
    {
        narratorText.text = "";

        foreach (char c in message)
        {
            narratorText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        int wordCount = message.Split(' ').Length;
        float readTime = Mathf.Max(2.0f, wordCount * 0.3f); 

        yield return new WaitForSeconds(readTime);
        narratorText.text = "";
    }
}