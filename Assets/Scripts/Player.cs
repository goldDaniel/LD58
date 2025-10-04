using System;
using System.Collections;
using TMPro;

[Serializable]
public class Player
{
    private int _maxEssence;
    public int MaxEssence;

	private int _currentEssence;
    public int CurrentEssence;

	private int _block;
    public int Block;
    

	public TextMeshProUGUI healthText;
	public TextMeshProUGUI essenceText;

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

	[NonSerialized] public int Strength = 0;
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
            // TODO draw cards
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
            //TODO Draw card and reduce cost to 0
        }
        if (card.cardTemplate.CurseEachPlay)
        {
            CurseEachPlay += 1;
        }

        yield return null;
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            //TODO die-
        }
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