using UnityEngine;
using TMPro;
using System.Collections;

public class CutsceneText : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.03f;

    public static CutsceneText Instance;

    void Awake()
    {
        Debug.Log("CutsceneText Awake — setting Instance");

        if (Instance != null && Instance != this)
        {
            Debug.Log("Duplicate CutsceneText destroyed");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public IEnumerator ShowText(string sentence)
    {
        textUI.text = "";

        foreach (char c in sentence)
        {
            textUI.text += c;

            yield return new WaitForSeconds(typingSpeed);
        }
    }
    
    public void ResetText()
    {
        Debug.Log("ResetText CALLED");
        StopAllCoroutines();
        textUI.text = "";
    }

    public IEnumerator ShowMultiple(params string[] sentences)
    {
        Debug.Log("ShowMultiple START — got " + sentences.Length + " sentences");

        ResetText();

        foreach (string s in sentences)
        {
            Debug.Log("Starting sentence: " + s);
            yield return StartCoroutine(ShowText(s));
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("ShowMultiple END");
        textUI.text = "";
    }
}