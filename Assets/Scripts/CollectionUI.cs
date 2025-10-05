using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CollectionUI : MonoBehaviour
{
    [SerializeField] RectTransform cardList;
    [SerializeField] CollectionListItem cardListPrefab;

    public List<CollectionListItem> cardListObjects;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadCards(CardType.Odin, CardType.Fates);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadCards(CardType type1, CardType type2)
    {
        foreach (CardTemplate card in GameProgress.Instance.collection.Keys)
        {
            if (card.Type == type1 || card.Type == type2)
            {
                var listItem = Instantiate(cardListPrefab, cardList);
                listItem.OnItemInitialize(card);
                cardListObjects.Add(listItem);
            }
        }
    }
}
