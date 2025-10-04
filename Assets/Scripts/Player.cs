using System;
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

	private int _maxHealth;
	public int MaxHealth
	{
		get => _maxHealth;
		set
		{
			_maxHealth = Math.Max(_maxHealth, 1);
			healthText.text = $"Health {CurrentHealth} / {_maxHealth}";
		}
	}

	private int _currentHealth;
	public int CurrentHealth
	{
		get => _currentHealth;
		set
		{
			_currentHealth = Math.Max(_currentHealth, 0);
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
}