using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    public static UIFader Instance { get; private set; }

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float timer = 0f;

        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            color.a = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                timer / fadeDuration);

            fadeImage.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
}