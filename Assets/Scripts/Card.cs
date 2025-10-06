using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	public Image cardBack;
	[SerializeField]
	public Image cardFront;

	[SerializeField]
	private Image cardIconPrefab;

	[SerializeField]
	private RectTransform cardIconRegion;

	[SerializeField]
	private CanvasGroup raycastBlocker;

	public bool Highlighted => dummy.gameObject.activeSelf;

	private int previousChildIndex = -1;
	private RectTransform initialParent;
	public RectTransform rectTransform;

	public bool EnableRaycast { get => raycastBlocker.blocksRaycasts; set => raycastBlocker.blocksRaycasts = value; }

	public CardTemplate cardTemplate;
	public int currentCost;
	public int originalCost;

	public TextMeshProUGUI title;
	public TextMeshProUGUI description;

	public Card dummy;

	public bool displayOnly = false;

	[SerializeField]
	public TextMeshProUGUI essenceCostText;

	[SerializeField]
	public Sprite damageSprite, deathChanceSprite, doomSprite, healSprite, strengthSprite, weakSprite, jinxSprite, confuseSprite, curseSprite, bloodyStrikeSprite, blockSprite, drawSprite, luckSprite, essenceSprite, fateSealedSprite;

	public void OnCardInitialize(CardTemplate card)
	{
		cardTemplate = card;
		cardFront.sprite = card.cardFront;
		title.text = card.CardName;
		description.text = card.CardDescription;
		currentCost = card.EssenceCost;
		essenceCostText.text = $"{currentCost}";
		originalCost = currentCost;

		dummy = Instantiate(this, UIController.Instance.transform);
		dummy.EnableRaycast = false;
		dummy.SetupIcons(card);

		dummy.enabled = false;
		dummy.transform.localScale = new Vector2(1.75f, 1.75f);
		dummy.gameObject.SetActive(false);

		SetupIcons(card);
	}

	public void UpdateCost(int cost)
	{
		currentCost = cost;
		essenceCostText.text = $"{cost}";
		dummy.essenceCostText.text = essenceCostText.text;
	}

	private void SetupIcons(CardTemplate card)
	{
		if (card.MinDamage != -1 || card.MaxDamage != -1)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = damageSprite;
		}
		if(card.BloodyStrike > 0)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = bloodyStrikeSprite;
		}
		if (card.Doomed > 0)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = doomSprite;
		}
		if(card.Jinxed)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = jinxSprite;
		}

		if(card.FateSealed)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = fateSealedSprite;
		}

		if (card.Confuse)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = confuseSprite;
		}
		if(card.Curse > 0)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = curseSprite;
		}
		if (card.DeathChance > 0)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = deathChanceSprite;
		}
		if (card.Weak > 0)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = weakSprite;
		}
		if(card.Block > 0)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = blockSprite;
		}
		if (card.Heal > 0)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = healSprite;
		}
		if (card.Draw > 0)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = drawSprite;
		}
		if(card.Strength > 0)
		{
			var icon = Instantiate(cardIconPrefab, cardIconRegion);
			icon.sprite = strengthSprite;
		}
	}

	public void SetInitialParent(RectTransform rt) => initialParent = rt;

	public void Update()
	{
		if (UIController.Instance.IsSelectedCard(this))
			dummy.gameObject.SetActive(false);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!displayOnly)
		{
			if (Highlighted && UIController.Instance.IsSelectedCard(null))
			{
				UIController.Instance.SetSelectedCard(this);
				dummy.gameObject.SetActive(false);
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!displayOnly)
		{
			if (UIController.Instance.IsSelectedCard(this))
			{
				if (Game.Instance.AttackEnemyWith(this))
				{
					EnableRaycast = false;
					UIController.Instance.SetSelectedCard(null);
					return;
				}

				UIController.Instance.SetSelectedCard(null);
				rectTransform.SetParent(initialParent);
				dummy.gameObject.SetActive(false);
			}
		}
	}

	public void SetPosition(Vector2 position) => rectTransform.position = position;

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!displayOnly)
		{
			if (UIController.Instance.IsSelectedCard(null))
				AudioManager.Instance.Play("Hover");

			previousChildIndex = transform.parent.GetSiblingIndex();

			var rect = dummy.GetComponent<RectTransform>();
			rect.position = this.rectTransform.position;

			dummy.gameObject.transform.SetAsLastSibling();
			dummy.gameObject.SetActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!displayOnly)
		{
			rectTransform.localScale = new Vector2(1.0f, 1.0f);

			dummy.gameObject.SetActive(false);
		}
	}

	public void SetInHand()
	{
		cardFront.gameObject.SetActive(true);
		cardBack.gameObject.SetActive(false);
		EnableRaycast = true;
	}

	public void SetInPile(RectTransform parent)
	{
		cardFront.gameObject.SetActive(false);
		cardBack.gameObject.SetActive(true);
		EnableRaycast = false;
		rectTransform.SetParent(parent);

		rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		rectTransform.pivot = new Vector2(0.5f, 0.5f);

		rectTransform.anchoredPosition = new Vector2(0, 0);
	}

	public void Discard()
	{
		Game.Instance.Discard(this);
	}
}

[Serializable]
public class CardGroup
{
	public int Size => cards.Count;

	[SerializeField]
	private List<Card> cards = new();
	public bool Contains(Card card) => cards.Contains(card);
	public void Remove(Card card) => cards.Remove(card);

	public void Add(Card card) => cards.Add(card);

	public void ForEach(Action<Card> action) => cards.ForEach(action);

	public void ForEachReverse(Action<Card> action)
	{
		for(int i = cards.Count - 1; i >= 0; --i)
			action(cards[i]);
	}

	public void Shuffle()
	{
		System.Random rng = new();

		int n = cards.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			Card value = cards[k];
			cards[k] = cards[n];
			cards[n] = value;
		}
	}

	public Card Draw()
	{
		Debug.Assert(cards.Count > 0, "Attempting to draw from empty card group");
		if(cards.Count == 0) 
			return null;

		var result = cards[0];
		cards.RemoveAt(0);
		return result;;
	}
}
