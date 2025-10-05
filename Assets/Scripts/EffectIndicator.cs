using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EffectType { Damage, Shield, Heal, Other, Curse };

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
    [SerializeField] private Sprite shieldIcon;
    [SerializeField] private Sprite healIcon;
    [SerializeField] private Sprite OtherIcon;
    [SerializeField] private Sprite CurseIcon;
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

    public IEnumerator DoEffectVisual(EffectType effectType, int value, bool isFade, string textOverride)
    {
        canvasGroup.alpha = 1;
        if (textOverride != null)
        {
            valueText.text = textOverride;
        }
        else
        {
            if (value >= 0)
            {
                valueText.text = $"{value}";
            }
            else
            {
                valueText.text = "";
            }
        }

        if (effectType == EffectType.Damage)
        {
            icon.sprite = damageIcon;
        }
        if (effectType == EffectType.Shield)
        {
            icon.sprite = shieldIcon;
        }
        if (effectType == EffectType.Heal)
        {
            icon.sprite = healIcon;
        }
        if (effectType == EffectType.Other)
        {
            icon.sprite = OtherIcon;
        }
        if (effectType == EffectType.Curse)
        {
            icon.sprite = CurseIcon;
        }
        if (isFade)
        {
            yield return Fade(fadeTime);
        }
        yield return null;
    }

    private IEnumerator Fade(float time)
    {
        float t = time;
        while (t > 0)
        {
            canvasGroup.alpha = Mathf.Clamp01(t / time);
            t -= Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
