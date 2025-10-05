using NUnit.Framework;
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

    public List<CardType> selectedCardTypes;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oldDecklist = GameProgress.Instance.currentDecklist.ToDictionary(entry => entry.Key, entry => entry.Value);
        loadCards(CardType.Odin, CardType.Fates);
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
    public void resetDecklist()
    {
        GameProgress.Instance.currentDecklist = oldDecklist.ToDictionary(entry => entry.Key, entry => entry.Value);
        foreach (CollectionListItem c in  cardListObjects)
        {
            c.SetQuantityText();
        }
    }
    public void BackWithoutSaving()
    {
        resetDecklist();
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
            //Display message that deck size must be 20
        }
    }
    public void loadCards(CardType type1, CardType type2)
    {
        foreach (CardTemplate card in GameProgress.Instance.collection.Keys)
        {
            if (card.Type == type1 || card.Type == type2)
            {
                var listItem = Instantiate(cardListPrefab, cardList);
                listItem.collection = this;
                listItem.OnItemInitialize(card);
                cardListObjects.Add(listItem);
            }
        }
    }

    public void selectGod(CardType type)
    {
        if (selectedCardTypes.Contains(type))
        {
            selectedCardTypes.Remove(type);
        } else if (selectedCardTypes.Count >= 2)
        {
            warningMessage.text = "You cannot select more than 2 gods in your deck";
        } else
        {
            selectedCardTypes.Add(type);
        }
    }
}
