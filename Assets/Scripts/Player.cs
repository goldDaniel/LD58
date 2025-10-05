using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public class Player
{
    private int _maxEssence = 3;
    public int MaxEssence
    {
        get => _maxEssence;
        set
        {
            _maxEssence = Math.Max(value, 1);
            essenceText.text = $"Essence {CurrentEssence} / {_maxEssence}";
        }
    }

	private int _currentEssence;
    public int CurrentEssence
    {
        get => _currentEssence;
        set
        {
            _currentEssence = Math.Max(value, 0);
            essenceText.text = $"Essence {_currentEssence} / {MaxEssence}";
        }
    }

    private int _block = 0;
    public int Block
    {
        get => _block;
        set
        {
            _block = Math.Max(value, 0);
            blockText.text = $"Block {_block}";
        }
    }


    public TextMeshProUGUI healthText;
	public TextMeshProUGUI essenceText;
    public TextMeshProUGUI blockText;

	private int _maxHealth = 100;
	public int MaxHealth
	{
		get => _maxHealth;
		set
		{
			_maxHealth = Math.Max(value, 1);
			healthText.text = $"Health {CurrentHealth} / {_maxHealth}";
		}
	}

	private int _currentHealth;
	public int CurrentHealth
	{
		get => _currentHealth;
		set
		{
			_currentHealth = Math.Max(value, 0);
			healthText.text = $"Health {_currentHealth} / {MaxHealth}";
		}
	}

    public TextMeshProUGUI StrengthText;
    private int _strength;
    public int Strength
    {
        get => _strength;
        set
        {
            _strength = Math.Max(value, 0);
            StrengthText.text = $"Strength: {_strength}";
        }
    }

	[NonSerialized] public int RepeatAllNext = 0;
    [NonSerialized] public int RepeatAllCurrentTurn = 0;
    [NonSerialized] public int doubleDamageHit = 0;
	[NonSerialized] public int PactOfPower = 0;
	[NonSerialized] public int PactOfSacrifice = 0;
	[NonSerialized] public int PowerCounter = 0;
	[NonSerialized] public int CurseEachPlay = 0;
	[NonSerialized] public int Lucky = 0;
    [NonSerialized] public int Curse = 0;
    [NonSerialized] public bool Lethargic = false;

    public IEnumerator ApplyEffectSequence(Card card)
    {
        if (card.cardTemplate.Draw > 0)
        {
            yield return Game.Instance.DrawCardFromDeck(false);
        }
        if (card.cardTemplate.Heal > 0)
        {
            Heal(card.cardTemplate.Heal);
        }
        if (card.cardTemplate.Block > 0)
        {
            Block += card.cardTemplate.Block;
        }
        if (card.cardTemplate.GetLucky)
        {
            Lucky += 1;
        }
        if (card.cardTemplate.Strength > 0)
        {
            Strength += card.cardTemplate.Strength;
        }
        if (card.cardTemplate.EssenceGain > 0)
        {
            CurrentEssence += card.cardTemplate.EssenceGain;
        }
        if (card.cardTemplate.Power)
        {
            PactOfPower += 1;
        }
        if (card.cardTemplate.Sacrifice)
        {
            PactOfSacrifice += 1;
        }
        if (card.cardTemplate.RepeatAllNext)
        {
            RepeatAllNext += 1;
        }
        if (card.cardTemplate.Foretell)
        {
            yield return Game.Instance.DrawCardFromDeck(true);
        }
        if (card.cardTemplate.CurseEachPlay)
        {
            CurseEachPlay += 1;
        }

        yield return null;
    }
    public IEnumerator TakeDamage(int damage)
    {
        var effect = GameObject.Instantiate(Game.Instance.effectPrefab,Game.Instance.playerDamageLocation);
        yield return effect.DoEffectVisual(EffectType.Damage, damage,true, null);
        GameObject.Destroy(effect.gameObject);
        if (Block >= damage)
        {
            Block -= damage;
        } else
        {
            CurrentHealth -= damage - Block;
            Block = 0;
            if (CurrentHealth <= 0)
            {
                //TODO die-
            }
        }
        yield return null;
        
    }
    public void Heal(int healing)
    {
        CurrentHealth += healing;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
}