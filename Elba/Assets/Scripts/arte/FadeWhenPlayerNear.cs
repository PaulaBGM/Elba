using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FadeWhenPlayerNear : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField, Range(0f, 1f)] private float fadedAlpha = 0.45f;
    [SerializeField] private float fadeSpeed = 6f;
    [SerializeField] private string playerTag = "Player";

    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    private float targetAlpha = 1f;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
            originalColors[i] = spriteRenderers[i].color;

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Update()
    {
        float currentAlpha = spriteRenderers.Length > 0 ? spriteRenderers[0].color.a : 1f;
        float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);

        ApplyAlpha(newAlpha);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            targetAlpha = fadedAlpha;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            targetAlpha = 1f;
    }

    private void ApplyAlpha(float alpha)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] == null)
                continue;

            Color c = originalColors[i];
            c.a = alpha;
            spriteRenderers[i].color = c;
        }
    }
}