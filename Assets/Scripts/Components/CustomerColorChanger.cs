using DG.Tweening;
using UnityEngine;

public class CustomerColorChanger : MonoBehaviour
{
    [SerializeField] private float colorChangeDuration = 0.5f;
    [SerializeField] private Color underWaterColor;
    private Color originalColor;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void UpdateColor(float air, float maxAir)
    {
        float normalizedAir = air / maxAir;
        Color targetColor = Color.Lerp(originalColor, underWaterColor, 1 - normalizedAir);
        spriteRenderer.DOColor(targetColor, colorChangeDuration).SetEase(Ease.Linear);
    }
}