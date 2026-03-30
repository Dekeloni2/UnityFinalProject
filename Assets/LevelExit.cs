using System.Collections;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private bool playerInRange = false;

    public FadeController fade; 
    public CutsceneText cutscene;
    public MusicPlayer music;

    private void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Debug.Log("LevelExit: key pressed, starting sequence");
            StartCoroutine(EnterDoorSequence());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("LevelExit: player entered");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("LevelExit: player left");
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator EnterDoorSequence()
    {
        yield return StartCoroutine(fade.FadeOut());
        
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
        
        if (music != null)
            yield return StartCoroutine(music.FadeOutMusic());

        yield return new WaitForSeconds(2f);
        
        yield return StartCoroutine(fade.FadeOut());
        
        QuitGame();
    }
}