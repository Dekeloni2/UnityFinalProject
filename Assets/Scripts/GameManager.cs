using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CutsceneText cutscene;

    private void Start()
    {
        
        CutsceneText.Instance.ResetText();
        StartCoroutine(cutscene.ShowMultiple(
            "Where am I?",
            "I guess I need to escape. Press the Arrows or WASD to move around",
            "Perhaps I need to get above this, Press Space to jump."
            
        ));
    }
}