using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject borderHighlight;

    [SerializeField]
    private Image cardImage;

    public bool Highlighted => borderHighlight.activeSelf;

    private RectTransform initialParent;
    private int intitialSiblingIndex; 
    public RectTransform rectTransform;

    public bool EnableRaycast { get => cardImage.raycastTarget; set => cardImage.raycastTarget = value; }

    public CardTemplate cardTemplate;

    public void Start()
    {
        intitialSiblingIndex = transform.GetSiblingIndex();
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
            transform.SetSiblingIndex(intitialSiblingIndex);
        }
    }

    public void SetPosition(Vector2 position) => rectTransform.position = position;

    public void OnPointerEnter(PointerEventData eventData) => borderHighlight.SetActive(true);

    public void OnPointerExit(PointerEventData eventData) => borderHighlight.SetActive(false);

}

public class CardGroup
{
    public int Size => cards.Count;

    private List<Card> cards = new();
    public bool Contains(Card card) => cards.Contains(card);
    public void Remove(Card card) => cards.Remove(card);

    public void Add(Card card) => cards.Add(card);

    public void Shuffle() => cards.Sort((x, y) => (int)(Random.value * int.MaxValue));

    public Card Draw()
    {
        if(cards.Count == 0) 
            return null;

        var result = cards[0];
        cards.RemoveAt(0);
        return result;;
    }
}
