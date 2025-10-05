using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public enum EffectType { Damage, Shield, Heal, Other, Curse, Strength };

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
	[SerializeField] private Sprite StrengthIcon;
	private RectTransform rectTransform;

	private bool fadeOut;

	public void Initialize(EffectType effectType, int value, bool isFade, string textOverride)
	{
		canvasGroup.alpha = 1;
		if (textOverride != null)
			valueText.text = textOverride.Replace("\n"," ");
		else
		{
			if (value >= 0)
				valueText.text = $"{value}";
			else
				valueText.text = "";
		}

		switch (effectType)
		{
			case EffectType.Damage:
				icon.sprite = damageIcon; break;
			case EffectType.Shield:
				icon.sprite = shieldIcon; break;
			case EffectType.Heal:
				icon.sprite = healIcon; break;
			case EffectType.Curse:
				icon.sprite = CurseIcon; break;
			case EffectType.Other:
				icon.sprite = OtherIcon; break;
			case EffectType.Strength:
				icon.sprite = StrengthIcon; break;
		}

		fadeOut = isFade;
	}

	public IEnumerator FadeIn(float time)
	{
		float t = 0;
		canvasGroup.alpha = 0f;
		while (t < time)
		{
			canvasGroup.alpha = Mathf.Clamp01(t / time);
			t += Time.deltaTime;
			yield return null;
		}
		canvasGroup.alpha = 1;
	}

	public IEnumerator FadeOut(float time)
    {
        float t = time;
		canvasGroup.alpha = 1f;
        while (t > 0)
        {
            canvasGroup.alpha = Mathf.Clamp01(t / time);
            t -= Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

	public IEnumerator MoveTo(Vector2 position)
	{
		var tween = transform.DOMove(position, 0.2f).SetEase(Ease.InQuint);
		while(tween.IsActive() && !tween.IsComplete())
			yield return null;
	}

	public IEnumerator FadeDestroy(List<EffectIndicator> group)
	{
		if (group != null)
			group.Remove(this);

		yield return FadeOut(0.1f);
		Destroy(this.gameObject);
	}
}
