using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject highlight;

    private RectTransform rectTransform;

	[SerializeField]
	private TextMeshProUGUI text;

	public int maxHealth;

	private int _currentHealth;
    public int CurrentHealth
	{
		get => _currentHealth;
		set 
		{
            _currentHealth = Math.Clamp(value, 0, maxHealth);
			text.text = $"HP: {_currentHealth}/{maxHealth}";
		}
	}

    public int Doom = 0;
    public bool Jinxed = false;
    public int Block = 0;
    public bool Confused = false;
    public int Weak = 0;
    public int BonusSouls = 0;
    public int Curse = 0;
    public int Strength = 0;

    public EnemyAttack Attacks = new EnemyAttack();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

	public void OnIntitialize(EnemyTemplate template)
	{
		maxHealth = template.MaxHealth;
		CurrentHealth = maxHealth;
        Attacks = template.AttackList;
        // TODO (rest of the things)
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
            BonusSouls += card.cardTemplate.Souls;
        }
        if (card.cardTemplate.DeathChance > 0)
        {
            if (UnityEngine.Random.Range(0, 100) > card.cardTemplate.DeathChance)
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
                    damage = Mathf.Max(damage, UnityEngine.Random.Range(card.cardTemplate.MinDamage, card.cardTemplate.MaxDamage));
                }
                damage *= 1<<Game.Instance.player.doubleDamageHit;
                repeatCount++;
            }
        } while (repeatCount <= card.cardTemplate.MultHit);
        if (card.cardTemplate.Doomed > 0)
        {
            Doom += card.cardTemplate.Doomed;
            if (Doom >= 3)
            {
                takeDamage(Doom * 20);
                Doom = 0;
            }
        }
        if (card.cardTemplate.Jinxed)
        {
            Jinxed = true;
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
            Confused = true;
        }
        if (card.cardTemplate.Weak > 0)
        {
            Weak += card.cardTemplate.Weak;
        }
        if (card.cardTemplate.Curse > 0)
        {
            Curse += card.cardTemplate.Curse;
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
        BonusSouls = 0;

        yield return null;
    }

    public void takeDamage(int damage)
    {
		CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            // Death Effect
        }
    }

    public void SetHighlight(bool active) => highlight.SetActive(active);
}
