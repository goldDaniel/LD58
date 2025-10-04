using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject highlight;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public bool IsIntersectingMouse()
    {
        Rect rect = MathUtils.RectTransformToScreenSpace(rectTransform);
        return rect.Contains(Mouse.current.position.value);
    }

    private void Update()
    {
        if (IsIntersectingMouse())
            Game.Instance.SelectEnemy(this);
        else
            Game.Instance.DeselectEnemy(this);
    }

    public void ApplyEffects(Card card)
    {

    }

    public void SetHighlight(bool active) => highlight.SetActive(active);
}
