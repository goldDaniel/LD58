using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CollectionUI : MonoBehaviour
{
	[SerializeField] RectTransform cardList;
	[SerializeField] CollectionListItem cardListPrefab;

	public List<CollectionListItem> cardListObjects;
	private Dictionary<CardTemplate, int> oldDecklist;
	[SerializeField] public TextMeshProUGUI deckSizeText;
	[SerializeField] Button backButton;
	[SerializeField] Button resetButton;
	[SerializeField] Button saveButton;
	[SerializeField] TextMeshProUGUI warningMessage;
	[SerializeField] Button odinButton;
	[SerializeField] Button mickiButton;
	[SerializeField] Button reaperButton;
	[SerializeField] Button anubisButton;
	[SerializeField] Button fatesButton;
	[SerializeField] RectTransform cardHolder;
	[SerializeField] Card cardDisplayPrefab;

	public Card visibleCard;
	private GameObject cardDisplay;

	public List<CardType> selectedCardTypes;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		oldDecklist = GameProgress.Instance.currentDecklist.ToDictionary(entry => entry.Key, entry => entry.Value);
		List<CardType> godsInDeck = GetCardTypeListFromDeck(GameProgress.Instance.currentDecklist);
		foreach (CardType type in godsInDeck)
		{
			selectGod(type);
		}
		//loadCards(new List<CardType> { CardType.Odin, CardType.Fates });
		odinButton.onClick.AddListener(() => selectGod(CardType.Odin));
		mickiButton.onClick.AddListener(() => selectGod(CardType.Micki));
		reaperButton.onClick.AddListener(() => selectGod(CardType.Reaper));
		anubisButton.onClick.AddListener(() => selectGod(CardType.Anubis));
		fatesButton.onClick.AddListener(() => selectGod(CardType.Fates));
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	Button getButtonFromCardType(CardType type)
	{
		if (type == CardType.Odin) { return odinButton; }
		if (type == CardType.Reaper) { return reaperButton; }
		if (type == CardType.Anubis) { return anubisButton; }
		if (type == CardType.Fates) { return fatesButton; }
		if (type == CardType.Micki) { return mickiButton; }
		return null;
	}
	List<CardType> GetCardTypeListFromDeck(Dictionary<CardTemplate, int> deck) {
		return deck.Select(entry => entry.Key.Type).Distinct().ToList();

	}
	public void DisplayCard(CardTemplate card)
	{
		HideCard();
		visibleCard = Instantiate(cardDisplayPrefab, cardHolder);
		visibleCard.OnCardInitialize(card);
		visibleCard.cardFront.gameObject.SetActive(true);
	}
	public void HideCard()
	{
		if (visibleCard != null)
		{
			Destroy(visibleCard.gameObject);
		}
	}
	public void resetDecklist()
	{
		List<CardType> godsInDeck = selectedCardTypes.ToList();
		List<CardType> godsInOldDeck = GetCardTypeListFromDeck(oldDecklist);
		GameProgress.Instance.currentDecklist = oldDecklist.ToDictionary(entry => entry.Key, entry => entry.Value);

		
		foreach (CardType type in godsInDeck)
		{
			selectGod(type);
		}
		foreach (CardType type in godsInOldDeck)
		{
			selectGod(type);
		}
		foreach (CollectionListItem c in  cardListObjects)
		{
			c.SetQuantityText();
		}
	}
	public void BackWithoutSaving()
	{
		GameProgress.Instance.currentDecklist = GameProgress.Instance.currentDecklist = oldDecklist.ToDictionary(entry => entry.Key, entry => entry.Value);
		SceneManager.LoadScene("Level Select");
		//Scene change
	}
	public void Save()
	{
		int deckSize = 0;
		foreach (var c in GameProgress.Instance.currentDecklist.Keys)
		{
			deckSize += GameProgress.Instance.currentDecklist[c];
		}
		if (deckSize == 20)
		{
			SceneManager.LoadScene("Level Select");
		}
		else
		{
			warningMessage.text = "Decks must have 20 cards in them";
		}
	}
	public void loadCards(List<CardType> types)
	{
		foreach (CollectionListItem c in cardListObjects)
		{
			if (!types.Contains(c.cardTemplate.Type) && GameProgress.Instance.currentDecklist.ContainsKey(c.cardTemplate))
			{
				GameProgress.Instance.RemoveCardFromDecklist(c.cardTemplate, GameProgress.Instance.currentDecklist[c.cardTemplate]);
			}
			Destroy(c.gameObject);
		}
		cardListObjects = new List<CollectionListItem>();
		setDecklistText();
		foreach (CardTemplate card in GameProgress.Instance.collection.Keys.OrderBy( k => k.Type))
		{
			if (types.Contains(card.Type))
			{
				var listItem = Instantiate(cardListPrefab, cardList);
				listItem.collection = this;
				listItem.OnItemInitialize(card);
				cardListObjects.Add(listItem);
			}
		}
	}
	public void setDecklistText()
	{
		int deckSize = 0;
		foreach (var c in GameProgress.Instance.currentDecklist.Keys)
		{
			deckSize += GameProgress.Instance.currentDecklist[c];
		}
		deckSizeText.text = $"Deck Size: {deckSize}/20";
	}


	public void selectGod(CardType type)
	{
		Button button = getButtonFromCardType(type);
		if (selectedCardTypes.Contains(type))
		{
			selectedCardTypes.Remove(type);
			toggleOutline(button);
			loadCards(selectedCardTypes);
			
		} else if (selectedCardTypes.Count >= 2)
		{
			warningMessage.text = "You cannot select more than 2 gods in your deck";
		} else
		{
			toggleOutline(button);
			selectedCardTypes.Add(type);
			loadCards(selectedCardTypes);
		}
	}
	public void toggleOutline(Button button)
	{
		button.gameObject.GetComponent<Outline>().enabled = !button.gameObject.GetComponent<Outline>().enabled;
	}
}
