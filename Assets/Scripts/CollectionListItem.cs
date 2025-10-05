using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class CollectionListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI cardNameText;
    [SerializeField] Button addCardButton;
    [SerializeField] Button removeCardButton;
    [SerializeField] TextMeshProUGUI cardQuantityText;
    [SerializeField] public CardTemplate cardTemplate;
    [SerializeField] Image background;

    public CollectionUI collection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        collection.DisplayCard(cardTemplate);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (collection.visibleCard.cardTemplate == cardTemplate)
        {
            collection.HideCard();
        }
    }

    public void SetQuantityText()
    {
        int inDeck = 0;
        int inCollection = 0;

        if(GameProgress.Instance.collection.ContainsKey(cardTemplate))
        {
            inCollection = GameProgress.Instance.collection[cardTemplate];
        }
        if (GameProgress.Instance.currentDecklist.ContainsKey(cardTemplate))
        {
            inDeck = GameProgress.Instance.currentDecklist[cardTemplate];
        }
        cardQuantityText.text = $"{inDeck}/{inCollection}";
        collection.setDecklistText();


    }
    private void addCard(CardTemplate card)
    {
        GameProgress.Instance.AddCardToDecklist(card);
        SetQuantityText();
    }
    private void removeCard(CardTemplate card)
    {
        GameProgress.Instance.RemoveCardFromDecklist(card, 1);
        SetQuantityText();
    }

    public void OnItemInitialize(CardTemplate card)
    {
        cardTemplate = card;
        cardNameText.text = card.CardName;
        SetQuantityText();
        addCardButton.onClick.AddListener(() => addCard(card));
        removeCardButton.onClick.AddListener(() => removeCard(card));
        if(card.Type == CardType.Odin)
        {
            background.color = new Color32(241,231,220,255);
        }
        if (card.Type == CardType.Reaper)
        {
            background.color = new Color32(168,181,171, 255);
        }
        if (card.Type == CardType.Fates)
        {
            background.color = new Color32(237, 211, 248, 255);
        }
        if (card.Type == CardType.Anubis)
        {
            background.color = new Color32(220, 229, 241, 255);   
        }
        if (card.Type == CardType.Micki)
        {
            background.color = new Color32(226, 190, 176, 255);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
