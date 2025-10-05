using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
            essenceText.text = $"{CurrentEssence} / {_maxEssence}";
        }
    }

	private int _currentEssence;
    public int CurrentEssence
    {
        get => _currentEssence;
        set
        {
            _currentEssence = Math.Max(value, 0);
            essenceText.text = $"{_currentEssence} / {MaxEssence}";
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

    public TextMeshProUGUI IncomingDamageText;
    private int _incomingDamage = 0;
    public int IncomingDamage
    {
        get => _incomingDamage;
        set
        {
            _incomingDamage = Math.Max(value, 0);
            IncomingDamageText.text = $"Incoming Damage: {_incomingDamage}";
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
			healthText.text = $"{CurrentHealth} / {_maxHealth}";
		}
	}

	private int _currentHealth;
	public int CurrentHealth
	{
		get => _currentHealth;
		set
		{
			_currentHealth = Math.Max(value, 0);
			healthText.text = $"{_currentHealth} / {MaxHealth}";
		}
	}

    public TextMeshProUGUI StrengthText;
    private int _strength = 0;
    public int Strength
    {
        get => _strength;
        set
        {
            _strength = Math.Max(value, 0);
            StrengthText.text = $"Strength: {_strength}";
        }
    }

    public TextMeshProUGUI CurseText;
    private int _curse = 0;
    public int Curse
    {
        get => _curse;
        set
        {
            _curse = Math.Max(value, 0);
            CurseText.text = $"Curse: {_curse}";
        }
    }

    public TextMeshProUGUI LuckyText;
    private int _lucky = 0;
    public int Lucky
    {
        get => _lucky;
        set
        {
            _lucky = Math.Max(value, 0);
            LuckyText.text = $"Lucky: {_lucky}";
        }
    }

    public TextMeshProUGUI LethargicText;
    private bool _lethargic = false;
    public bool Lethargic
    {
        get => _lethargic;
        set
        {
            if (_lethargic)
            {
                LethargicText.text = "Lethargic!";
            }
            else
            {
                LethargicText.text = string.Empty;
            }
        }
    }

    public TextMeshProUGUI CurseEachPlayText;
    private int _curseEachPlay = 0;
    public int CurseEachPlay
    {
        get => _curseEachPlay;
        set
        {
            _curseEachPlay = Math.Max(value, 0);
            CurseEachPlayText.text = $"Afflict: {_curseEachPlay}";
        }
    }


    [NonSerialized] public int RepeatAllNext = 0;
    [NonSerialized] public int RepeatAllCurrentTurn = 0;
    [NonSerialized] public int doubleDamageHit = 0;
	[NonSerialized] public int PactOfPower = 0;
	[NonSerialized] public int PactOfSacrifice = 0;
	[NonSerialized] public int PowerCounter = 0;

    public IEnumerator ApplyEffectSequence(Card card)
    {
        if (card.cardTemplate.SelfDamage > 0)
        {
            yield return TakeDamage(card.cardTemplate.SelfDamage);
        }
        if (card.cardTemplate.Draw > 0)
        {
            yield return Game.Instance.DrawHand(card.cardTemplate.Draw);
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
        if (card.cardTemplate.NextTurnDamage > 0)
        {
            IncomingDamage += card.cardTemplate.NextTurnDamage;
        }

        yield return null;
    }
    public IEnumerator TakeDamage(int damage)
    {

        // TODO (danielg): animate the damage indicator from the enemy to the player

        //var effect = GameObject.Instantiate(Game.Instance.effectPrefab,Game.Instance.playerDamageLocation);
        //      effect.Initialize(EffectType.Damage, damage,true, null);
        //      GameObject.Destroy(effect.gameObject);


        while (damage > 0 && Block > 0)
        {
            Block--;
            damage--;
        }

        CurrentHealth -= damage;

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