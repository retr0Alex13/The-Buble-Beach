using DG.Tweening;
using UnityEngine;

public class CustomerColorChanger : MonoBehaviour
{
    [SerializeField] private float colorChangeDuration = 0.5f;
    [SerializeField] private Color underWaterColor;
    [SerializeField] private Color originalColor;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Tween colorTween;

    private void Awake()
    {
        //originalColor = spriteRenderer.color;
        spriteRenderer.color = originalColor;
    }

    public void UpdateColor(float air, float maxAir)
    {
        float normalizedAir = air / maxAir;
        Color targetColor = Color.Lerp(originalColor, underWaterColor, 1 - normalizedAir);

        colorTween?.Kill();
        colorTween = spriteRenderer.DOColor(targetColor, colorChangeDuration).SetEase(Ease.Linear);
    }
}