using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	private GameObject borderHighlight;

	[SerializeField]
	private Image cardBack;
	[SerializeField]
	private Image cardFront;

	[SerializeField]
	private CanvasGroup raycastBlocker;

	public bool Highlighted => borderHighlight.activeSelf;

	private RectTransform initialParent;
	public RectTransform rectTransform;

	public bool EnableRaycast { get => raycastBlocker.blocksRaycasts; set => raycastBlocker.blocksRaycasts = value; }

	public CardTemplate cardTemplate;
	public int currentCost;

	public TextMeshProUGUI title;
	public TextMeshProUGUI description;


	public void OnCardInitialize(CardTemplate card)
	{
		cardTemplate = card;
		cardFront.sprite = card.cardFront;
		title.text = card.name;
		description.text = card.CardDescription;
		currentCost = card.EssenceCost;
	}

	public void SetInitialParent(RectTransform rt) => initialParent = rt;
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if (Highlighted && UIController.Instance.IsSelectedCard(null))
		{
			UIController.Instance.SetSelectedCard(this);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
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
		}
	}

	public void SetPosition(Vector2 position) => rectTransform.position = position;

	public void OnPointerEnter(PointerEventData eventData) => borderHighlight.SetActive(true);

	public void OnPointerExit(PointerEventData eventData) => borderHighlight.SetActive(false);

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

	public void Shuffle() => cards.Sort((x, y) => (int)(UnityEngine.Random.value * int.MaxValue));

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
