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
    [SerializeField] CardTemplate cardTemplate;

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
        int deckSize = 0;

        if(GameProgress.Instance.collection.ContainsKey(cardTemplate))
        {
            inCollection = GameProgress.Instance.collection[cardTemplate];
        }
        if (GameProgress.Instance.currentDecklist.ContainsKey(cardTemplate))
        {
            inDeck = GameProgress.Instance.currentDecklist[cardTemplate];
        }
        cardQuantityText.text = $"{inDeck}/{inCollection}";
        foreach (var c in GameProgress.Instance.currentDecklist.Keys)
        {
            deckSize += GameProgress.Instance.currentDecklist[c];
        }
        collection.deckSizeText.text = $"Deck Size: {deckSize}/20";


    }
    private void addCard(CardTemplate card)
    {
        GameProgress.Instance.AddCardToDecklist(card);
        SetQuantityText();
    }
    private void removeCard(CardTemplate card)
    {
        GameProgress.Instance.RemoveCardFromDecklist(card);
        SetQuantityText();
    }

    public void OnItemInitialize(CardTemplate card)
    {
        cardTemplate = card;
        cardNameText.text = card.name;
        SetQuantityText();
        addCardButton.onClick.AddListener(() => addCard(card));
        removeCardButton.onClick.AddListener(() => removeCard(card));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
