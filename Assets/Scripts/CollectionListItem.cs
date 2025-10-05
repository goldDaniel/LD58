using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class CollectionListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cardNameText;
    [SerializeField] Button addCardButton;
    [SerializeField] Button removeCardButton;
    [SerializeField] TextMeshProUGUI cardQuantityText;
    [SerializeField] CardTemplate cardTemplate;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
