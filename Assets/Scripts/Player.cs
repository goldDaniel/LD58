using System;
using TMPro;

[Serializable]
public class Player
{
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
	[NonSerialized] public int doubleDamageHit = 0;
	[NonSerialized] public int PactOfPower = 0;
	[NonSerialized] public int PactOfSacrifice = 0;
	[NonSerialized] public int SacrificeCounter = 0;
	[NonSerialized] public int CurseEachPlay = 0;
	[NonSerialized] public int Lucky = 0;
}