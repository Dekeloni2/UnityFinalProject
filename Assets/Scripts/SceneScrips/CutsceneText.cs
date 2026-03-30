using UnityEngine;
using TMPro;
using System.Collections;

public class CutsceneText : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.03f;
    
    public AudioSource audioSource;
    public AudioClip blipSound;

    public static CutsceneText Instance;

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
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

            if (c != ' ' && blipSound != null && audioSource != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.2f);
                audioSource.PlayOneShot(blipSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void ResetText()
    {
        StopAllCoroutines();
        textUI.text = "";
    }

    public IEnumerator ShowMultiple(params string[] sentences)
    {

        ResetText();

        foreach (string s in sentences)
        {
            yield return StartCoroutine(ShowText(s));
            yield return new WaitForSeconds(1f);
        }
        
        textUI.text = "";
    }
}