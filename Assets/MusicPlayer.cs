using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeSpeed = 1f;

    void Start()
    {
        audioSource.Play();
    }

    public IEnumerator FadeOutMusic()
    {
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        audioSource.Stop();
    }
}