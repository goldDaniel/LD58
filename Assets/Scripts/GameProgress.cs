
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgress : MonoSingleton<GameProgress>
{
	public LevelTemplate selectedLevel = null;

	public bool hasCompletedTutorial = false;

	public Dictionary<LevelTemplate, bool> completedLevels = new();

	public List<LevelTemplate> anubisLevels = new();
	public List<LevelTemplate> fateLevels = new();
	public List<LevelTemplate> mickiLevels = new();
	public List<LevelTemplate> odinLevels = new();
	public List<LevelTemplate> reaperLevels = new();

    [SerializeField] public List<CardTemplate> odinStartingCards;
    [SerializeField] public List<CardTemplate> mickiStartingCards;
    [SerializeField] public List<CardTemplate> anubisStartingCards;
    [SerializeField] public List<CardTemplate> reaperStartingCards;
    [SerializeField] public List<CardTemplate> fatesStartingCards;
    [SerializeField] public List<CardTemplate> allCards;

    public Dictionary<CardTemplate, int> currentDecklist = new();
    public Dictionary<CardTemplate, int> collection = new();
    public int pendingRandomCards = 0;
    public int pendingOdinCards = 0;
    public int pendingMickiCards = 0;
    public int pendingAnubisCards = 0;
    public int pendingReaperCards = 0;
    public int pendingFatesCards = 0;

    public override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this);

		foreach (var a in anubisLevels)
			completedLevels.Add(a, false);

		foreach (var a in fateLevels)
			completedLevels.Add(a, false);

		foreach (var a in mickiLevels)
			completedLevels.Add(a, false);

		foreach (var a in odinLevels)
			completedLevels.Add(a, false);

		foreach (var a in reaperLevels)
			completedLevels.Add(a, false);
		
	}
    public void Start()
    {
        InitialCollection();
        InitialDecklist(new List<CardType> { CardType.Odin, CardType.Micki });
    }

    public void SelectLevel(LevelTemplate level)
	{
		selectedLevel = level;
		SceneManager.LoadScene("GameScene");
	}

	public void AddCardToCollection(CardTemplate card)
	{
		if (collection.ContainsKey(card))
		{
			collection[card] += 1;
		}
		else
		{
			collection.Add(card,1);
		}
	}
    public CardTemplate GetRandomCard()
    {
        List<CardTemplate> cards = allCards;
        int index = UnityEngine.Random.Range(0, cards.Count);
        return cards[index];
    }
    public CardTemplate GetRandomGodCard(CardType type)
    {
        List<CardTemplate> cards = allCards.Where(x => x.Type == type).ToList();
        int index = UnityEngine.Random.Range(0, cards.Count);
        return cards[index];
    }
	public void AddCardToDecklist(CardTemplate card)
	{
		int currentCount = 0;
		if (currentDecklist.ContainsKey(card))
		{
			currentCount = currentDecklist[card];
            if (collection[card] > currentDecklist[card])
            {
                currentDecklist[card] += 1;
            }
        }
        else
        {
            currentDecklist.Add(card, 1);
        }
    }
	public void RemoveCardFromDecklist(CardTemplate card, int count)
	{
        if (currentDecklist.ContainsKey(card))
		{
			if (currentDecklist[card] <= count)
			{
				currentDecklist.Remove(card);
			}
            else
            {
                currentDecklist[card] -= count;
            }
		}
    }
	public void InitialCollection()
	{
		foreach (CardTemplate card in odinStartingCards)
		{
			AddCardToCollection(card);
		}
        foreach (CardTemplate card in mickiStartingCards)
        {
            AddCardToCollection(card);
        }
        foreach (CardTemplate card in reaperStartingCards)
        {
            AddCardToCollection(card);
        }
        foreach (CardTemplate card in anubisStartingCards)
        {
            AddCardToCollection(card);
        }
        foreach (CardTemplate card in fatesStartingCards)
        {
            AddCardToCollection(card);
        }
    }
	public void InitialDecklist(List<CardType> types)
	{
		if(types.Contains( CardType.Odin))
		{
			foreach (CardTemplate card in odinStartingCards)
			{
				AddCardToDecklist(card);
			}
		}
        if (types.Contains(CardType.Micki))
        {
            foreach (CardTemplate card in mickiStartingCards)
            {
                AddCardToDecklist(card);
            }
        }
        if (types.Contains(CardType.Reaper))
        {
            foreach (CardTemplate card in reaperStartingCards)
            {
                AddCardToDecklist(card);
            }
        }
        if (types.Contains(CardType.Anubis))
        {
            foreach (CardTemplate card in anubisStartingCards)
            {
                AddCardToDecklist(card);
            }
        }
        if (types.Contains(CardType.Fates))
        {
            foreach (CardTemplate card in fatesStartingCards)
            {
                AddCardToDecklist(card);
            }
        }
    }

	internal void CompleteCurrentLevel()
	{
		if(selectedLevel != null)
			completedLevels[selectedLevel] = true;
	}
    public void AddRewardsFromCurrentLevel()
    {
        if (selectedLevel != null)
        {
            pendingRandomCards += selectedLevel.randomReward;
            pendingOdinCards += selectedLevel.odinReward;
            pendingMickiCards += selectedLevel.mickiReward;
            pendingAnubisCards += selectedLevel.anubisReward;
            pendingReaperCards += selectedLevel.reaperReward;
            pendingFatesCards += selectedLevel.fatesReward;
        }

    }
}
