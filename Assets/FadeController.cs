using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1f;

    public IEnumerator FadeOut()
    {
        Color c = fadeImage.color;

        while (c.a < 1f)
        {
            c.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = c;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        Color c = fadeImage.color;

        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = c;
            yield return null;
        }
    }
}