using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private Image image;

	[SerializeField]
    private GameObject highlight;

    private RectTransform rectTransform;

	[SerializeField]
	private TextMeshProUGUI healthText;

    public int maxHealth;

    private int _currentHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Math.Clamp(value, 0, maxHealth);
            healthText.text = $"HP: {_currentHealth}/{maxHealth}";
        }
    }

    [SerializeField]
    private TextMeshProUGUI blockText;

    private int _block = 0;
    public int Block
    {
        get => _block;
        set
        {
            _block = Math.Max(value, 0);
            blockText.text = $"Block: {_block}";
        }
    }

    [SerializeField]
    private TextMeshProUGUI doomText;

    private int _doom = 0;
    public int Doom
    {
        get => _doom;
        set
        {
            _doom = Math.Max(value, 0);
            doomText.text = $"Doom: {_doom}";
        }
    }

    [SerializeField]
    private TextMeshProUGUI curseText;

    private int _curse = 0;
    public int Curse
    {
        get => _curse;
        set
        {
            _curse = Math.Max(value, 0);
            curseText.text = $"Curse: {_curse}";
        }
    }

    [SerializeField]
    private TextMeshProUGUI strengthText;

    private int _strength = 0;
    public int Strength
    {
        get => _strength;
        set
        {
            _strength = Math.Max(value, 0);
            curseText.text = $"Strength: {_strength}";
        }
    }

    [SerializeField]
    private TextMeshProUGUI weakText;

    private int _weak = 0;
    public int Weak
    {
        get => _weak;
        set
        {
            _weak = Math.Max(value, 0);
            weakText.text = $"Weak: {_weak}";
        }
    }

    [SerializeField]
    private TextMeshProUGUI jinxText;

    private bool _jinxed = false;
    public bool Jinxed
    {
        get => _jinxed;
        set
        {
            if (value)
            {
                jinxText.text = "Jinxed!";
            }
            else
            {
                jinxText.text = string.Empty;
            }   
        }
    }

    [SerializeField]
    private TextMeshProUGUI confusedText;

    private bool _confused = false;
    public bool Confused
    {
        get => _confused;
        set
        {
            if (value)
            {
                confusedText.text = "Confused!";
            }
            else
            {
                confusedText.text = string.Empty;
            }
        }
    }

    public int BonusSouls = 0;

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
        Block = template.Block;
        Doom = 0;
        Curse = 0;
        Strength = 0;
        Weak = 0;
        Jinxed = false;
        Confused = false;

		image.sprite = template.sprite;

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
                yield return takeDamage(1000);
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
                yield return takeDamage(damage);
            }
        } while (repeatCount <= card.cardTemplate.MultHit);
        if (card.cardTemplate.Doomed > 0)
        {
            Doom += card.cardTemplate.Doomed;
            if (Doom >= 3)
            {
                yield return takeDamage(Doom * 20);
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
            yield return Game.Instance.player.TakeDamage(currentHealth * card.cardTemplate.BloodyStrike / 100);
            yield return takeDamage(currentHealth * card.cardTemplate.BloodyStrike / 100);
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

    public IEnumerator takeDamage(int damage)
    {
		CurrentHealth -= damage;
        return null;
    }

    public void SetHighlight(bool active) => highlight.SetActive(active);
}
