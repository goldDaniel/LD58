using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject borderHighlight;

    public bool Highlighted => borderHighlight.activeSelf;

    private RectTransform initialParent;
    private Vector2 initialPosition;
    public RectTransform rectTransform;

    public CardTemplate cardTemplate;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.localPosition;
        initialParent = rectTransform.parent.GetComponent<RectTransform>();
    }
    
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
            UIController.Instance.SetSelectedCard(null);
            rectTransform.SetParent(initialParent);
            rectTransform.localPosition = initialPosition;
        }
    }

    public void SetPosition(Vector2 position) => rectTransform.position = position;

    public void OnPointerEnter(PointerEventData eventData) => borderHighlight.SetActive(true);

    public void OnPointerExit(PointerEventData eventData) => borderHighlight.SetActive(false);

}


public class CardGroup
{
    List<Card> cards = new();
}