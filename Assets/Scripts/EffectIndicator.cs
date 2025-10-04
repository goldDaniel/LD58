using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EffectType { Damage };

public class EffectIndicator : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI valueText;
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Sprite damageIcon;
    [SerializeField]

    private float fadeTime = 0.4f;
    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator DoEffectVisual(EffectType effectType, int value, bool isFade)
    {
        canvasGroup.alpha = 1;
        valueText.text = $"{value}";

        if (effectType == EffectType.Damage)
        {
            icon.sprite = damageIcon;
        }
        if (isFade)
        {
            yield return Fade(fadeTime);
        }
        yield return null;
    }

    private IEnumerator Fade(float time)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
        }
        return null;
    }
}
