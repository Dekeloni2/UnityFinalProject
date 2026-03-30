using UnityEngine;
using TMPro;
using System.Collections;

public class CutsceneText : MonoBehaviour
{
    public TextMeshProUGUI textUI;         // UI element used to display the dialogue text
    public float typingSpeed = 0.03f;      // Delay between each character for the typing effect

    public AudioSource audioSource;        // Audio source used to play the typing sound
    public AudioClip blipSound;            // Sound played for each typed character

    public static CutsceneText Instance;   // Singleton instance so other scripts can easily access this class

    void Awake()
    {
        // Basic singleton pattern to ensure only one CutsceneText exists in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public IEnumerator ShowText(string sentence)
    {
        // Clear the text before starting the typing animation
        textUI.text = "";

        // Type the sentence one character at a time
        foreach (char c in sentence)
        {
            textUI.text += c;

            // Play a small "blip" sound for each character (except spaces)
            if (c != ' ' && blipSound != null && audioSource != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.2f); // Slight pitch variation for a more natural sound
                audioSource.PlayOneShot(blipSound);
            }

            // Wait before typing the next character
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void ResetText()
    {
        // Stop any ongoing typing animations and clear the text immediately
        StopAllCoroutines();
        textUI.text = "";
    }

    public IEnumerator ShowMultiple(params string[] sentences) //This doesn't work properly but I still wanted to implement something like this
    {
        // Make sure no previous text is still on screen
        ResetText();

        // Display each sentence one after another
        foreach (string s in sentences)
        {
            yield return StartCoroutine(ShowText(s));  // Type the sentence
            yield return new WaitForSeconds(1f);       // Small pause before the next line
        }

        // Clear the text after all lines are done
        textUI.text = "";
    }
}