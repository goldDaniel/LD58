using Assets.Scripts;
using DG.Tweening;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			var enemy = Instantiate(enemyPrefab);
			enemy.OnIntitialize(enemyTemplate);
			activeEnemies.Add(enemy);

			enemy.GetComponent<RectTransform>().SetParent(enemyContainer);
		}

		foreach(var c in defaultDeck)
		{
			var card = Instantiate(cardPrefab);
			card.OnCardInitialize(c);
			card.SetInitialParent(handContainer);
			card.rectTransform.SetParent(deckLocation);

			card.SetInPile();

			deck.Add(card);
		}
		deck.Shuffle();

		yield return DrawHand();
	}

	IEnumerator DrawHand()
	{
		while (hand.Size < handSize)
		{
			if(deck.Size == 0)
				yield return RefillDeck();

			Card card = deck.Draw();
			card.gameObject.SetActive(true);
			card.SetInHand();

			var tween = card.rectTransform.DOMove(handContainer.position, 0.2f).SetEase(Ease.InCirc);
			while (tween.IsActive() && !tween.IsComplete())
				yield return null;

			card.rectTransform.SetParent(handContainer);
			hand.Add(card);
		}
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
		if (attackInProgress)
			return false;

		if (selectedEnemy != null)
		{
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

	IEnumerator AttackPlayerSequence(Enemy attacker, List<Enemy> otherEnemies, Player player)
	{
		yield return null;
	}

	IEnumerator AttackEnemySeqeunce(Enemy enemy, Card card)
	{
		Debug.Assert(hand.Contains(card), "Attempting to attack with a card not in hand!");
		
		hand.Remove(card);

		yield return enemy.ApplyEffectSequence(card);

		// animate to discard pile
		var tween = card.rectTransform.DOMove(discardLocation.position, 0.2f).SetEase(Ease.InCirc);
		while(tween.IsActive() && !tween.IsComplete())
		{
			yield return null;
		}

		discard.Add(card);
		card.rectTransform.SetParent(discardLocation);
		card.SetInPile();

		attackInProgress = false;
	}
}