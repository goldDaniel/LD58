using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoSingleton<UIController>
{
	private RectTransform rectTransform;

	private Card selectedCard;

	public override void Awake()
	{
		base.Awake();
		rectTransform = GetComponent<RectTransform>();
	}

	public void SetSelectedCard(Card card)
	{
		selectedCard = card;
		if(card != null)
		{
			card.rectTransform.SetParent(rectTransform);
			card.transform.SetSiblingIndex(1);
		}
	}

	public bool IsSelectedCard(Card card) => selectedCard == card;

	public void Update()
	{
		if(selectedCard != null)
		{
			var mousePos = Mouse.current.position.value;
			selectedCard.rectTransform.position = mousePos;
			selectedCard.SetPosition(mousePos);
		}
	}
}
