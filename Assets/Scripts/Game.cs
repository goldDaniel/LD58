using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoSingleton<Game>
{
	public List<Enemy> activeEnemies = new();

	private Enemy selectedEnemy = null;

	private int handSize = 3;
	private int deckSize = 20;
    private CardGroup hand = new();

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
	
	
	private bool attackInProgress = false;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

	public IEnumerator Start()
	{
		for(int i = 0; i < deckSize; ++i)
		{
            var card = Instantiate(cardPrefab);

            var hue = Random.Range(0f, 1f);
            var saturation = Random.Range(0.6f, 0.8f);
            var brightness = Random.Range(0.8f, 1f);
            card.GetComponent<Image>().color = Color.HSVToRGB(hue, saturation, brightness);
            card.SetInitialParent(handContainer);

			card.rectTransform.SetParent(deckLocation);

			deck.Add(card);
        }
		deck.Shuffle();

		yield return DrawHand();
	}

	IEnumerator DrawHand()
	{
		while (hand.Size < handSize)
		{
			Card card = deck.Draw();
			card.gameObject.SetActive(true);

            var tween = card.rectTransform.DOMove(MathUtils.RectTransformToScreenSpace(handContainer).position, 0.4f).SetEase(Ease.InCirc);
            while (tween.IsActive() && !tween.IsComplete())
                yield return null;

            card.rectTransform.SetParent(handContainer);
            hand.Add(card);
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

		attackInProgress = false;
	}
}