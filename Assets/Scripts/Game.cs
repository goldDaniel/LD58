using Assets.Scripts;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	public static Game Instance;
	public static bool HasInstance => Instance != null;

	public Player player = new();

	public List<Enemy> activeEnemies = new();

	private Enemy selectedEnemy = null;

	private int handSize = 5;
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

	[SerializeField]
	public EffectIndicator effectPrefab;

	[SerializeField]
	public RectTransform playerDamageLocation;

	[SerializeField] public List<CardTemplate> odinStartingCards;
    [SerializeField] public List<CardTemplate> mickiStartingCards;
    [SerializeField] public List<CardTemplate> anubisStartingCards;
    [SerializeField] public List<CardTemplate> reaperStartingCards;
    [SerializeField] public List<CardTemplate> fatesStartingCards;

    private int enemyTurnIndex;
	public bool IsPlayerTurn { get; private set; }

	public void Awake()
	{
		Instance = this;
		enemyTurnIndex = -1;
		IsPlayerTurn = true;
	}

	public void OnDestroy()
	{
		Instance = null;
	}

	public void LoadLevel() => StartCoroutine(LoadLevel_Internal());

	private IEnumerator LoadLevel_Internal()
	{
		player.CurrentHealth = player.MaxHealth;
		player.CurrentEssence = player.MaxEssence;

		AudioManager.Instance.Play("CombatMusic", 2f);

		var level = GameProgress.Instance.selectedLevel ?? testLevel;
		foreach (var enemyTemplate in level.Enemies)
		{
            SpawnEnemy(enemyTemplate);
        }

		foreach(var c in defaultDeck)
		{
			var card = Instantiate(cardPrefab, deckLocation);
			card.OnCardInitialize(c);
			card.SetInitialParent(handContainer);
			card.SetInPile(deckLocation);

			deck.Add(card);
		}

        yield return ShuffleDeckAnimation();
        deck.Shuffle();

		yield return OnTurnStart(true);
	}

	private void SpawnEnemy(EnemyTemplate enemyTemplate)
	{
        var enemy = Instantiate(enemyPrefab, enemyContainer);
        enemy.OnIntitialize(enemyTemplate);
        activeEnemies.Add(enemy);
    }

	public IEnumerator DrawHand(int CardsToDraw)
	{
		CardsToDraw -= (player.Lethargic ? 1 : 0);

        for (int i = 0; i < CardsToDraw; i++)
		{
            yield return DrawCardFromDeck(false);
        }
		player.Lethargic = false;
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

		AudioManager.Instance.Play("Deal");

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

			var tween = card.rectTransform.DOMove(deckLocation.anchoredPosition, 0.15f).SetEase(Ease.InBounce);
			while (tween.IsActive() && !tween.IsComplete())
				yield return null;

			card.SetInPile(deckLocation);
		}

		yield return ShuffleDeckAnimation();
		deck.Shuffle();
	}

	IEnumerator ShuffleDeckAnimation()
	{
		const float delayIncrement = 0.05f;
		float delay = 0;

		AudioManager.Instance.Play("Shuffle");
		List<Tweener> tweens = new();
		deck.ForEachReverse(c =>
		{
			c.rectTransform.rotation = Quaternion.identity;
			var tween = c.rectTransform.DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360)
										.SetEase(Ease.InOutBounce)
										.SetDelay(delay)
										.OnComplete(() => c.rectTransform.rotation = Quaternion.identity);
			tweens.Add(tween);
			delay += delayIncrement;
		});

		while(tweens.Count > 0)
		{
			foreach (var t in tweens)
			{
				if (t.IsActive() && !t.IsComplete())
					yield return null;
			}

			tweens = tweens.Where(x => x.IsActive() && !x.IsComplete()).ToList();
		}
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
		if (attackInProgress)
			return false;

        if (player.CurrentEssence >= card.currentCost && selectedEnemy != null)
        {
            player.CurrentEssence -= card.currentCost;
			
			var enemy = selectedEnemy;
			DeselectEnemy(selectedEnemy);

			attackInProgress = true;
			StartCoroutine(AttackEnemySeqeunce(enemy, card));

            return true;
        }
        

		return false;
	}

	public void OnTurnEnd()
	{
		Debug.Assert(IsPlayerTurn, "Cannot end turn!");
		if (!IsPlayerTurn)
			return;

		endTurnButton.interactable = false;
		IsPlayerTurn = false;
		enemyTurnIndex = 0;

		StartCoroutine(DoEnemyTurn());
	}

	public IEnumerator OnTurnStart(bool firstTurn)
	{
		endTurnButton.interactable = true;
		IsPlayerTurn = true;
		enemyTurnIndex = -1;
		player.CurrentEssence = player.MaxEssence;
		player.Block = 0;
		player.RepeatAllCurrentTurn = player.RepeatAllNext;
		player.RepeatAllNext = 0;
		foreach (Enemy e in activeEnemies)
		{
			yield return PrepareAttack(e);
		}

		int Draw = firstTurn ? 5 : 2;

		yield return StartCoroutine(DrawHand(Draw));

        yield return ApplyStatusEffects();
	}

	public IEnumerator ApplyStatusEffects()
	{
		if (player.Curse > 0)
		{
            yield return player.TakeDamage(player.Curse);
			player.Curse--;
		}
	}

	IEnumerator PrepareAttack(Enemy enemy)
	{
		yield return enemy.OnTurnStart();

        var Attack = enemy.Attacks.Attacks.FirstOrDefault();
        if (Attack.ClearNegative)
        {
            yield return DisplayEnemyEffect(enemy, EffectType.Other, 0, "Cleanse");
        }

        if (Attack.SpawnEnemy != null)
        {
            yield return DisplayEnemyEffect(enemy, EffectType.Other, 0, "Spawn");
        }

        if (Attack.ApplyLethergy)
        {
            yield return DisplayEnemyEffect(enemy, EffectType.Other, 0, "Lethargy");
        }


        if (Attack.Heal != -1)
        {
            if (Attack.TargetAllEnemies)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Heal, 0, $"Heal All\n{Attack.Heal}");
            }
            else if (Attack.TargetAllOtherEnemies)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Heal, 0, $"Heal All Other\n{Attack.Heal}");
            }
            else if (Attack.TargetRandomEnemy)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Heal, 0, $"Heal Random\n{Attack.Heal}");
            }
            else if (Attack.TargetSelf)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Heal, 0, $"Heal Self\n{Attack.Heal}");
            }
        }

        if (Attack.Block != -1)
        {
            if (Attack.TargetAllEnemies)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Shield, 0, $"Give All\n{Attack.Block}");
            }
            else if (Attack.TargetAllOtherEnemies)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Shield, 0, $"Give All Other\n{Attack.Block}");
            }
            else if (Attack.TargetRandomEnemy)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Shield, 0, $"Give Random\n{Attack.Block}");
            }
            else if (Attack.TargetSelf)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Shield, 0, $"Give Self\n{Attack.Block}");
            }
        }

        if (Attack.Curse != -1)
        {
            yield return DisplayEnemyEffect(enemy, EffectType.Curse, 0, $"Curse\n{Attack.Curse}");
        }

        if (Attack.Strength != -1)
        {
            if (Attack.TargetAllEnemies)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Other, 0, $"Strengthen {Attack.Strength} All");
            }
            else if (Attack.TargetAllOtherEnemies)

            {
                yield return DisplayEnemyEffect(enemy, EffectType.Other, 0, $"Strengthen {Attack.Strength} Other");
            }
            else if (Attack.TargetRandomEnemy)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Other, 0, $"Strengthen {Attack.Strength} Random");
            }
            else if (Attack.TargetSelf)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Other, 0, $"Strengthen {Attack.Strength} Self");
            }
        }

        if (Attack.Damage != -1)
        {
            int TotalDamage = Attack.Damage;

            if (enemy.Weak != -1)
            {
                TotalDamage -= enemy.Weak;
            }

            if (enemy.Strength != -1)
            {
                TotalDamage += enemy.Strength;
            }

            if (Attack.MassBonus)
            {
                TotalDamage += activeEnemies.Count - 1;
            }

            if (Attack.BlockBonus)
            {
                TotalDamage += enemy.Block;
            }

            if (Attack.MissingHealthBonus)
            {
                TotalDamage += (enemy.CurrentHealth / enemy.maxHealth) * 10;
            }


            if (Attack.NumberOfAttacks > 1)
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Damage, Attack.Damage, $"{Attack.Damage}x{Attack.NumberOfAttacks}");
            }
            else
            {
                yield return DisplayEnemyEffect(enemy, EffectType.Damage, Attack.Damage, $"Damage\n{Attack.Damage}" );
            }
        }
    }

	IEnumerator DisplayEnemyEffect(Enemy enemy, EffectType type, int value, string textOverride)
	{
		if (enemy.effect1 == null)
		{
			enemy.effect1 = Instantiate(effectPrefab, enemy.effect1Location);
			yield return enemy.effect1.DoEffectVisual(type, value,false, textOverride);

		} 
		else if (enemy.effect2 == null)
        {
            enemy.effect2 = Instantiate(effectPrefab, enemy.effect2Location);
            yield return enemy.effect2.DoEffectVisual(type, value, false,textOverride);

        }

		yield return null;
	}

	IEnumerator DoEnemyTurn()
	{
		while(enemyTurnIndex < activeEnemies.Count)
		{
			var active = activeEnemies[enemyTurnIndex];
			var others = activeEnemies.Where(e => e != active).ToList();
			yield return AttackPlayerSequence(active, others, player);

			if (active.effect1 != null)
			{
                Destroy(active.effect1.gameObject);
                active.effect1 = null;
            }
			if (active.effect2 != null)
			{
                Destroy(active.effect2.gameObject);
                active.effect2 = null;
            }
           
			enemyTurnIndex++;
		}

		CheckDeadEnemies();
		CheckWinLoss();

        enemyTurnIndex = -1;
		yield return OnTurnStart(false);
	}

	public IEnumerator NextAttack(Enemy enemy, bool prepare)
	{
        //End Attack Block

        EnemyAttackTemplate OldAttack = enemy.Attacks.Attacks[0];
        enemy.Attacks.Attacks.RemoveAt(0);
        enemy.Attacks.Attacks.Add(OldAttack);
        if (prepare)
        {
            yield return PrepareAttack(enemy);
        }
    }

	//Ben
	public void CheckDeadEnemies()
	{
		List<Enemy> savedEnemies = new List<Enemy>();

		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (activeEnemies[i].CurrentHealth <= 0)
			{
				savedEnemies.Add(activeEnemies[i]);
			}
		}

        foreach (var Enemy in savedEnemies)
		{
			Destroy(Enemy.gameObject);
			activeEnemies.Remove(Enemy);
		}
	}

	//Ben
	IEnumerator AttackPlayerSequence(Enemy attacker, List<Enemy> otherEnemies, Player player)
	{
		var Attack = attacker.Attacks.Attacks.FirstOrDefault();

		if (attacker.Curse > 0)
		{
			yield return attacker.takeDamage(attacker.Curse);
			attacker.Curse--;
		}

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

		if (attacker.Jinxed)
		{
			if (Attack.TargetAllEnemies || Attack.TargetAllOtherEnemies)
			{
				Attack.TargetAllEnemies = false;
				Attack.TargetAllOtherEnemies = false;
				Attack.TargetRandomEnemy = true;
			}
		}

		List<Enemy> targets = new();
		if (Attack.TargetAllEnemies)
			targets.AddRange(activeEnemies);
		else if (Attack.TargetAllOtherEnemies)
			targets.AddRange(otherEnemies);
		else if (Attack.TargetRandomEnemy)
			targets.Add(activeEnemies[UnityEngine.Random.Range(0, otherEnemies.Count - 1)]);
		else if (Attack.TargetSelf)
			targets.Add(attacker);

		if (Attack.Heal != -1)
		{
			foreach (var enemy in targets)
				enemy.CurrentHealth += Attack.Heal;
		}

		if (Attack.Block != -1)
		{
			foreach (var enemy in targets)
				enemy.Block += Attack.Block;
		}

		if (Attack.Curse != -1)
		{
			player.Curse += Attack.Curse;
		}

		if (Attack.Strength != -1)
		{
			foreach (var enemy in targets)
				enemy.Strength += Attack.Strength;
		}

		if (Attack.Damage != -1)
		{
			int TotalDamage = Attack.Damage;

			if (attacker.Weak != -1)
				TotalDamage -= attacker.Weak;
			if (attacker.Strength != -1)
				TotalDamage += attacker.Strength;
			if (Attack.MassBonus)
				TotalDamage += otherEnemies.Count;
			if (Attack.BlockBonus)
				TotalDamage += attacker.Block;
			if (Attack.MissingHealthBonus)
				TotalDamage += (attacker.CurrentHealth / attacker.maxHealth) * 10;

			var IsConfused = false;
			if (attacker.Confused)
			{
				var ConfusedChance = 50;
				IsConfused = UnityEngine.Random.Range(0, 100) < ConfusedChance;
			}

			int attackcount = Attack.NumberOfAttacks > 1 ? Attack.NumberOfAttacks : 1;
			for (int i = 0; i < attackcount; i++)
			{
				if (IsConfused)
					yield return attacker.takeDamage(TotalDamage);
				else
					yield return player.TakeDamage(TotalDamage);
			}
		}

		yield return NextAttack(attacker, false);
	}

public void Discard(Card card)
	{
		hand.Remove(card);
		discard.Add(card);
		card.SetInPile(discardLocation);
	}

	IEnumerator AttackEnemySeqeunce(Enemy target, Card card)
	{
		Debug.Assert(hand.Contains(card), "Attempting to attack with a card not in hand!");

        hand.Remove(card);

		bool targetAll = card.cardTemplate.TargetAllEnemies;

		List<Enemy> enemies = new();
		if (targetAll)
			enemies.AddRange(activeEnemies);
		else
			enemies.Add(target);

		foreach(var enemy in enemies)
		{
			if (enemy.CurrentHealth <= 0)
				continue;

			// animate to attack
			int iterations = (card.cardTemplate.MultHit <= 1) ? 1 : card.cardTemplate.MultHit;
			for (int i = 0; i < iterations; ++i)
			{
				var enemyRect = enemy.GetComponent<RectTransform>();
				var initialPosition = enemyRect.position.xy() - new Vector2(0, 500);
				var initialTween = card.rectTransform.DOMove(initialPosition, 0.15f).SetEase(Ease.OutCubic);
				while (initialTween.IsActive() && !initialTween.IsComplete())
					yield return null;

				yield return new WaitForSeconds(0.1f);

				var finalPosition = enemyRect.position;
				var tween = card.rectTransform.DOMove(finalPosition, 0.3f).SetEase(Ease.InBack);
				while (tween.IsActive() && !tween.IsComplete())
					yield return null;

				initialTween = card.rectTransform.DOMove(initialPosition, 0.15f).SetEase(Ease.OutCubic);
				while (initialTween.IsActive() && !initialTween.IsComplete())
					yield return null;

				yield return new WaitForSeconds(0.1f);
			}

			int currentRepeat = 0;
			do
			{
				currentRepeat++;
				if (player.PactOfPower > 0)
				{
					player.PowerCounter++;
					if (player.PowerCounter >= 3)
					{
						yield return player.TakeDamage(3 * player.PactOfPower);
						player.CurrentEssence += player.PactOfPower;
					}
				}

				yield return player.ApplyEffectSequence(card);

				yield return enemy.ApplyEffectSequence(card, player.Strength);
			} while (currentRepeat <= player.RepeatAllCurrentTurn);


			attackInProgress = false;

			CheckDeadEnemies();
		}

		// animate to discard pile
		{
			var tween = card.rectTransform.DOMove(discardLocation.position, 0.2f).SetEase(Ease.InCirc);
			while (tween.IsActive() && !tween.IsComplete())
				yield return null;
		}
		discard.Add(card);
		card.SetInPile(discardLocation);

		CheckWinLoss();
    }

	public void CheckWinLoss()
	{
        if (player.CurrentHealth <= 0)
        {
			SceneManager.LoadScene("Level Select");
        }

        else if (activeEnemies.Count == 0)
        {
            GameProgress.Instance.CompleteCurrentLevel();
            SceneManager.LoadScene("Level Select");
        }
    }
}