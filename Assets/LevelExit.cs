using System.Collections;
using UnityEngine;

public class PlayerDoor : MonoBehaviour
{
    private bool playerInRange = false;

    public FadeController fade; 
    public CutsceneText cutscene;
    public MusicPlayer music;

    private void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            StartCoroutine(EnterDoorSequence());
        }
    }

    private IEnumerator EnterDoorSequence()
    {
        // פייד אאוט
        yield return StartCoroutine(fade.FadeOut());

        // המתנה קטנה
        yield return new WaitForSeconds(1f);

        // הצגת הסצנה
        yield return StartCoroutine(cutscene.ShowMultiple(
            "I really don't know why I appeared here all of a sudden",
            "All of my memories are blank",
            "But I hope exploring this factory will help me understand myself better",
            "Why did I get these powers?" ,
            "How did I get here?",
            "What is outside the factory?",
            "These questions will remain unanswered unless I act...",
            ".........."
        ));

        // פייד אאוט של המשפט האחרון
        yield return StartCoroutine(fade.FadeOut());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
    }
    
}