using Assets.Scripts;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoSingleton<Game>
{
	public Player player = new();

	public List<Enemy> activeEnemies = new();

	private Enemy selectedEnemy = null;

	private int handSize = 3;
	private CardGroup hand = new();

	[SerializeField]
	private List<CardTemplate> defaultDeck;

	[SerializeField]
	private RectTransform handContainer;

	[SerializeField] 
	private Card cardPrefab;

	[SerializeField]
	private RectTransform deckLocation;

	private CardGroup deck = new();

	[SerializeField]
	private RectTransform discardLocation;
	private CardGroup discard = new();

	[SerializeField]
	private LevelTemplate testLevel;

	private bool attackInProgress = false;

	[SerializeField]
	private RectTransform enemyContainer;

	[SerializeField]
	private Enemy enemyPrefab;

	[SerializeField]
	private Button endTurnButton;

	private int enemyTurnIndex;
	public bool IsPlayerTurn { get; private set; }

	public override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this);
		enemyTurnIndex = -1;
		IsPlayerTurn = true;
	}

	public IEnumerator Start()
	{
		foreach (var enemyTemplate in testLevel.Enemies)
		{
            SpawnEnemy(enemyTemplate);
        }

		foreach(var c in defaultDeck)
		{
			var card = Instantiate(cardPrefab);
			card.OnCardInitialize(c);
			card.SetInitialParent(handContainer);
			card.SetInPile(deckLocation);

			deck.Add(card);
		}
		deck.Shuffle();

		yield return DrawHand();
	}

	private void SpawnEnemy(EnemyTemplate enemyTemplate)
	{
        var enemy = Instantiate(enemyPrefab);
        enemy.OnIntitialize(enemyTemplate);
        activeEnemies.Add(enemy);

        enemy.GetComponent<RectTransform>().SetParent(enemyContainer);
    }

	IEnumerator DrawHand()
	{
		while (hand.Size < handSize)
		{
			yield return DrawCardFromDeck(false);
		}
	}

	public IEnumerator DrawCardFromDeck(bool isFree)
	{
        if (deck.Size == 0)
            yield return RefillDeck();

        Card card = deck.Draw();
		if (isFree)
		{
			card.currentCost = 0;
		}
        card.gameObject.SetActive(true);
        card.SetInHand();

        var tween = card.rectTransform.DOMove(handContainer.position, 0.2f).SetEase(Ease.InCirc);
        while (tween.IsActive() && !tween.IsComplete())
            yield return null;

        card.rectTransform.SetParent(handContainer);
        hand.Add(card);
    }

	IEnumerator RefillDeck()
	{
		while (discard.Size > 0)
		{
			var card = discard.Draw();
			deck.Add(card);

			var tween = card.rectTransform.DOMove(deckLocation.position, 0.2f).SetEase(Ease.InCirc);
			while (tween.IsActive() && !tween.IsComplete())
				yield return null;
		}

		deck.Shuffle();
	}

	public void SelectEnemy(Enemy enemy)
	{
		if (attackInProgress)
			return;
		
		if (UIController.Instance.IsSelectedCard(null))
		{
			DeselectEnemy(enemy);
			return;
		}

		if (selectedEnemy == enemy)
			return;

		if(enemy != null)
			enemy.SetHighlight(true);

		selectedEnemy = enemy;
	}

	public void DeselectEnemy(Enemy enemy)
	{
		if (attackInProgress)
			return;

		if (selectedEnemy == enemy)
		{
			selectedEnemy = null;
			enemy.SetHighlight(false);
		}
	}

	public bool AttackEnemyWith(Card card)
	{
		int currentRepeat = 0;

		if (attackInProgress)
			return false;

        if (player.CurrentEssence >= card.currentCost && selectedEnemy != null)
        {
            player.CurrentEssence -= card.currentCost;
			currentRepeat++;
			do
			{
				if (player.PactOfPower > 0)
				{
					player.PowerCounter++;
					if (player.PowerCounter >= 3)
					{
						player.TakeDamage(3 * player.PactOfPower);
						player.CurrentEssence += player.PactOfPower;
					}
				}
				var enemy = selectedEnemy;
				DeselectEnemy(selectedEnemy);

				attackInProgress = true;
				StartCoroutine(AttackEnemySeqeunce(enemy, card));

			} while (currentRepeat <= player.RepeatAllCurrentTurn);

            return true;
        }
        

		return false;
	}

	public void OnTurnEnd()
	{
		endTurnButton.interactable = false;
		IsPlayerTurn = false;
		enemyTurnIndex = 0;

		StartCoroutine(DoEnemyTurn());
	}

	public void OnTurnStart()
	{
		endTurnButton.interactable = true;
		IsPlayerTurn = true;
		enemyTurnIndex = -1;
		player.CurrentEssence = player.MaxEssence;
		player.Block = 0;
		player.RepeatAllCurrentTurn = player.RepeatAllNext;
		player.RepeatAllNext = 0;

		StartCoroutine(DrawHand());
	}

	IEnumerator DoEnemyTurn()
	{
		while(enemyTurnIndex < activeEnemies.Count)
		{
			var active = activeEnemies[enemyTurnIndex];
			var others = activeEnemies.Where(e => e != active).ToList();
			yield return AttackPlayerSequence(active, others, player);
			enemyTurnIndex++;
		}

		enemyTurnIndex = -1;
		OnTurnStart();
	}


	//Ben
	IEnumerator AttackPlayerSequence(Enemy attacker, List<Enemy> otherEnemies, Player player)
	{
        var Attack = attacker.Attacks.Attacks.FirstOrDefault();

        //Start Attack Block
        if (Attack.ClearNegative)
        {
			attacker.Doom = 0;
			attacker.Jinxed = false;
			attacker.Confused = false;
			attacker.Weak = 0;
			attacker.Curse = 0;
        }

		if (Attack.SpawnEnemy != null)
		{
            SpawnEnemy(Attack.SpawnEnemy);
        }

		if (Attack.ApplyLethergy)
		{
			player.Lethargic = true;
        }

		if (Attack.Heal != -1)
		{
			if (Attack.TargetAllEnemies)
			{
                attacker.CurrentHealth += Attack.Heal;
                foreach (var enemy in otherEnemies)
                {
                    enemy.CurrentHealth += Attack.Heal;
                }
            }
			else if (Attack.TargetAllOtherEnemies)
			{
				foreach (var enemy in otherEnemies)
				{
					enemy.CurrentHealth += Attack.Heal;
				}
			}
			else if (Attack.TargetRandomEnemy)
			{
				int index = UnityEngine.Random.Range(0, otherEnemies.Count-1);
				otherEnemies[index].CurrentHealth += Attack.Heal;
            }
			else if (Attack.TargetSelf)
			{
                attacker.CurrentHealth += Attack.Heal;
            }
		}

		if (Attack.Block != -1)
		{
            if (Attack.TargetAllEnemies)
            {
                attacker.Block += Attack.Block;
                foreach (var enemy in otherEnemies)
                {
                    enemy.Block += Attack.Block;
                }
            }
            else if (Attack.TargetAllOtherEnemies)
            {
                foreach (var enemy in otherEnemies)
                {
                    enemy.Block += Attack.Block;
                }
            }
            else if (Attack.TargetRandomEnemy)
            {
                int index = UnityEngine.Random.Range(0, otherEnemies.Count - 1);
                otherEnemies[index].Block += Attack.Block;
            }
            else if (Attack.TargetSelf)
            {
                attacker.Block += Attack.Block;
            }
        }

        if (Attack.Curse != -1)
        {
            player.Curse = Attack.Curse;
        }

		if (Attack.Strength != -1)
		{
            if (Attack.TargetAllEnemies)
            {
                attacker.Strength += Attack.Strength;
                foreach (var enemy in otherEnemies)
                {
                    enemy.Strength += Attack.Strength;
                }
            }
            else if (Attack.TargetAllOtherEnemies)
            {
                foreach (var enemy in otherEnemies)
                {
                    enemy.Strength += Attack.Strength;
                }
            }
            else if (Attack.TargetRandomEnemy)
            {
                int index = UnityEngine.Random.Range(0, otherEnemies.Count - 1);
                otherEnemies[index].Strength += Attack.Strength;
            }
            else if (Attack.TargetSelf)
            {
                attacker.Strength += Attack.Strength;
            }
        }

		if (Attack.Damage != -1)
		{
			int TotalDamage = Attack.Damage;

			if (attacker.Strength != -1)
			{
				TotalDamage += attacker.Strength;
			}

			if (Attack.MassBonus)
			{
				TotalDamage += otherEnemies.Count;
			}

            if (Attack.BlockBonus)
            {
				TotalDamage += attacker.Block;
            }

            if (Attack.MissingHealthBonus)
            {
				TotalDamage += (attacker.CurrentHealth / attacker.maxHealth) * 10;
            }

			if (Attack.NumberOfAttacks > 1)
			{
				for (int i = 0; i < Attack.NumberOfAttacks; i++)
				{
					PlayerDamage(TotalDamage, player);
				}
			}
			else
			{
				PlayerDamage(TotalDamage, player);
			}
        }

        //End Attack Block

        EnemyAttackTemplate OldAttack = attacker.Attacks.Attacks[0];
		attacker.Attacks.Attacks.RemoveAt(0);
        attacker.Attacks.Attacks.Add(OldAttack);

        yield return null;
	}

	private void PlayerDamage(int Damage, Player player)
	{
		player.CurrentHealth -= Damage;
	}

	IEnumerator AttackEnemySeqeunce(Enemy enemy, Card card)
	{
		Debug.Assert(hand.Contains(card), "Attempting to attack with a card not in hand!");

        hand.Remove(card);

		yield return player.ApplyEffectSequence(card);

		yield return enemy.ApplyEffectSequence(card);

		// animate to discard pile
		var tween = card.rectTransform.DOMove(discardLocation.position, 0.2f).SetEase(Ease.InCirc);
		while(tween.IsActive() && !tween.IsComplete())
		{
			yield return null;
		}

		discard.Add(card);
		card.SetInPile(discardLocation);
		attackInProgress = false;
	}
}