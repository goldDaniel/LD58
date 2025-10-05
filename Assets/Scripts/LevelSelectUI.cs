using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
	public List<Button> anubisButtons;
	public List<Button> fateButtons;
	public List<Button> mickiButtons;
	public List<Button> odinButtons;
	public List<Button> reaperButtons;

	public Image rewardPanel;
	public RectTransform rewardHolder;
	public Card cardDisplayPrefab;

	public List<Card> cardsList;

	public void Start()
	{
		SetupButtons(anubisButtons, GameProgress.Instance.anubisLevels);
		SetupButtons(fateButtons, GameProgress.Instance.fateLevels);
		SetupButtons(mickiButtons, GameProgress.Instance.mickiLevels);
		SetupButtons(odinButtons, GameProgress.Instance.odinLevels);
		SetupButtons(reaperButtons, GameProgress.Instance.reaperLevels);

		SetUnlockedLevels(anubisButtons, GameProgress.Instance.anubisLevels);
		SetUnlockedLevels(fateButtons, GameProgress.Instance.fateLevels);
		SetUnlockedLevels(mickiButtons, GameProgress.Instance.mickiLevels);
		SetUnlockedLevels(odinButtons, GameProgress.Instance.odinLevels);
		SetUnlockedLevels(reaperButtons, GameProgress.Instance.reaperLevels);
		//GameProgress.Instance.pendingRandomCards = 3;
		GetRewards();
	}

	void SetupButtons(List<Button> buttons, List<LevelTemplate> levels)
	{
		for (int i = 0; i < buttons.Count; ++i)
		{
			var index = i;
			buttons[i].onClick.AddListener(() => GameProgress.Instance.SelectLevel(levels[index]));
			buttons[i].interactable = i == 0;
		}
	}

	void GetRewards()
	{
		if (GameProgress.Instance.pendingRandomCards > 0 ||
			GameProgress.Instance.pendingOdinCards > 0 ||
			GameProgress.Instance.pendingMickiCards > 0 ||
			GameProgress.Instance.pendingReaperCards > 0 ||
			GameProgress.Instance.pendingAnubisCards > 0 ||
			GameProgress.Instance.pendingFatesCards > 0)
		{
			List<CardTemplate> obtainedCards = new List<CardTemplate>();
			for (int i = 0; i < GameProgress.Instance.pendingRandomCards; i++)
			{
				obtainedCards.Add(GameProgress.Instance.GetRandomCard());
			}
            for (int i = 0; i < GameProgress.Instance.pendingOdinCards; i++)
            {
                obtainedCards.Add(GameProgress.Instance.GetRandomGodCard(CardType.Odin));
            }
            for (int i = 0; i < GameProgress.Instance.pendingMickiCards; i++)
            {
                obtainedCards.Add(GameProgress.Instance.GetRandomGodCard(CardType.Micki));
            }
            for (int i = 0; i < GameProgress.Instance.pendingAnubisCards; i++)
            {
                obtainedCards.Add(GameProgress.Instance.GetRandomGodCard(CardType.Anubis));
            }
            for (int i = 0; i < GameProgress.Instance.pendingReaperCards; i++)
            {
                obtainedCards.Add(GameProgress.Instance.GetRandomGodCard(CardType.Reaper));
            }
            for (int i = 0; i < GameProgress.Instance.pendingFatesCards; i++)
            {
                obtainedCards.Add(GameProgress.Instance.GetRandomGodCard(CardType.Fates));
            }
			rewardPanel.gameObject.SetActive(true);
			foreach (CardTemplate card in obtainedCards)
			{
				GameProgress.Instance.AddCardToCollection(card);
				var cardObject = Instantiate(cardDisplayPrefab, rewardHolder);
				cardObject.OnCardInitialize(card);
				cardObject.displayOnly = true;
				cardObject.cardFront.gameObject.SetActive(true);
				cardsList.Add(cardObject);
			}
            GameProgress.Instance.pendingRandomCards = 0;
			GameProgress.Instance.pendingOdinCards = 0;
			GameProgress.Instance.pendingMickiCards = 0;
			GameProgress.Instance.pendingReaperCards = 0;
			GameProgress.Instance.pendingAnubisCards = 0;
			GameProgress.Instance.pendingFatesCards = 0;
		}
    }
	public void closePanelClick()
	{
		
		StartCoroutine(closePanel());
		
    }
	public IEnumerator closePanel()
	{
		yield return FadeOutPanel(0.5f);
        rewardPanel.gameObject.SetActive(false);
        foreach (Card c in cardsList)
        {
            Destroy(c.gameObject);
        }
        cardsList.Clear();
    }
    public IEnumerator FadeOutPanel(float time)
    {
		CanvasGroup canvas = rewardPanel.gameObject.GetComponent<CanvasGroup>();
		float t = time;
		float currentAlpha = canvas.alpha;
        while (t > 0)
        {
            canvas.alpha = Mathf.Clamp01(t * currentAlpha / time);
            t -= Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 0;
    }


    public void SetUnlockedLevels(List<Button> buttons, List<LevelTemplate> levels)
	{
		for (int i = 0; i < levels.Count - 1; ++i)
		{
			var unlocked = GameProgress.Instance.completedLevels[levels[i]];
			buttons[i + 1].interactable = unlocked;
		}
	}
    public void GoToEditDeck()
    {
        SceneManager.LoadScene("Collection");
    }
}
