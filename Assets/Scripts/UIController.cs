
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
        card.rectTransform.SetParent(rectTransform);
    }

    public bool IsSelectedCard(Card card) => selectedCard == card;

    public void Update()
    {
        if(selectedCard != null)
        {
            var mousePos = Mouse.current.position.value;
            selectedCard.SetPosition(mousePos);
        }
    }
}
