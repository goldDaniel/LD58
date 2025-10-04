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
    private int doom = 0;
    private bool jinxed = false;
    private int block = 0;
    private bool confused = false;
    private int weak = 0;
    private int bonusSouls = 0;

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
        
        if (card.cardTemplate.Souls > 0)
        {
            bonusSouls += card.cardTemplate.Souls;
        }
        if (card.cardTemplate.DeathChance > 0)
        {
            if (Random.Range(0, 100) > card.cardTemplate.DeathChance)
            {
                takeDamage(1000);
            }
        }
        int repeatCount = 0;
        do
        {
            if (card.cardTemplate.MinDamage > 0 && card.cardTemplate.MaxDamage > 0)
            {
                int damage = 0;
                for (int j = 0; j < Game.Instance.player.Lucky + 1; j++)
                {
                    damage = Mathf.Max(damage, Random.Range(card.cardTemplate.MinDamage, card.cardTemplate.MaxDamage));
                }
                damage *= 1<<Game.Instance.player.doubleDamageHit;
                repeatCount++;
            }
        } while (repeatCount <= card.cardTemplate.MultHit);
        if (card.cardTemplate.Doomed > 0)
        {
            doom += card.cardTemplate.Doomed;
            if (doom >= 3)
            {
                takeDamage(doom *= 20);
                doom = 0;
            }
        }
        if (card.cardTemplate.Jinxed)
        {
            jinxed = true;
        }
        if (card.cardTemplate.FateSealed)
        {
            // Change to next action in list
        }
        if (card.cardTemplate.BloodyStrike > 0)
        {
            int currentHealth = Game.Instance.player.CurrentHealth;
            // Have player take damage
            takeDamage(currentHealth * card.cardTemplate.BloodyStrike / 100);
        }
        if (card.cardTemplate.Confuse)
        {
            confused = true;
        }
        if (card.cardTemplate.Weak > 0)
        {
            weak += card.cardTemplate.Weak;
        }

        /*
        if (SomeCardEffectApplies)
        {
            yield return DoTheAnimationForEffect(); (daniel will take care of this, leave comment saying it needs to be implemented)
            
            this.health -= ......
            this.hasStatusEffect = true;
            etc....
        }

         */
        //Bonus souls only apply for this attack
        bonusSouls = 0;

        yield return null;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            // Death Effect
        }
    }

    public void SetHighlight(bool active) => highlight.SetActive(active);
}
