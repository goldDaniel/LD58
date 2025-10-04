using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject highlight;

    private RectTransform rectTransform;

    private int health;

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

    public IEnumerator ApplyEffectSequence(Card card)
    {

        if (card.cardTemplate.)
        /*
        if (SomeCardEffectApplies)
        {
            yield return DoTheAnimationForEffect(); (daniel will take care of this, leave comment saying it needs to be implemented)
            
            this.health -= ......
            this.hasStatusEffect = true;
            etc....
        }

         */


        yield return null;
    }

    public void SetHighlight(bool active) => highlight.SetActive(active);
}
