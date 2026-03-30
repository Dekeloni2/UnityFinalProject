using System.Collections;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private bool playerInRange = false;      // True when the player is standing inside the exit trigger

    public FadeController fade;              // Handles screen fade-in and fade-out effects
    public CutsceneText cutscene;            // Handles cutscene dialogue text
    public MusicPlayer music;                // Controls background music fading

    private void Update()
    {
        // If the player is inside the exit area and presses the interact key, start the exit sequence
        if (playerInRange && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Debug.Log("LevelExit: key pressed, starting sequence");
            StartCoroutine(EnterDoorSequence());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect when the player enters the exit trigger
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("LevelExit: player entered");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Detect when the player leaves the exit trigger
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("LevelExit: player left");
        }
    }

    private void QuitGame()
    {
        // Quit the game. In the editor, stop play mode instead.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator EnterDoorSequence()
    {
        // Fade the screen to black before starting the cutscene
        yield return StartCoroutine(fade.FadeOut());

        // Display a sequence of cutscene dialogue lines
        yield return StartCoroutine(cutscene.ShowMultiple(
            "I really don't know why I appeared here all of a sudden",
            "All of my memories are blank",
            "But I hope exploring this factory will help me understand myself better",
            "Why did I get these powers?",
            "How did I get here?",
            "What is outside the factory?",
            "These questions will remain unanswered unless I act...",
            "....."
        ));

        // Fade out the music if a music player exists
        if (music != null)
            yield return StartCoroutine(music.FadeOutMusic());

        // Small pause before the final fade
        yield return new WaitForSeconds(2f);

        // Fade out again for dramatic effect before quitting
        yield return StartCoroutine(fade.FadeOut());

        // Exit the game
        QuitGame();
    }
}