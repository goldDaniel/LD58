using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoSingleton<Game>
{
	public List<Enemy> activeEnemies = new();

	private Enemy selectedEnemy = null;

	private int handSize = 3;
    private CardGroup hand = new();

	[SerializeField]
	private RectTransform cardContainer;

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

	public void Start()
	{
		for (int i = 0; i < handSize; i++)
		{
			var card = Instantiate(cardPrefab);

			var hue = Random.Range(0f, 1f);
            var saturation = Random.Range(0.6f, 0.8f);
			var brightness = Random.Range(0.8f, 1f);
            card.GetComponent<Image>().color = Color.HSVToRGB(hue, saturation, brightness);

			card.rectTransform.SetParent(cardContainer);
			card.SetInitialParent(cardContainer);
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
        var targetPosition = discardLocation.position + new Vector3(-card.rectTransform.rect.width / 2f, card.rectTransform.rect.height / 2f, 0);
        var tween = card.rectTransform.DOMove(discardLocation.position, 0.2f).SetEase(Ease.InCirc);
		while(tween.IsActive() && !tween.IsComplete())
		{
			yield return null;
		}

		discard.Add(card);

		attackInProgress = false;
	}
}