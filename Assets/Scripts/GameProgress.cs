
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgress : MonoSingleton<GameProgress>
{
	public LevelTemplate selectedLevel = null;

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

    public Dictionary<CardTemplate, int> currentDecklist = new();
    public Dictionary<CardTemplate, int> collection = new();

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
	public void RemoveCardFromDecklist(CardTemplate card)
	{
        if (currentDecklist.ContainsKey(card))
		{
			if (currentDecklist[card] == 1)
			{
				currentDecklist.Remove(card);
			}
            else
            {
                currentDecklist[card] -= 1;
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
	public void InitialDecklist(CardType type1, CardType type2)
	{
		if(type1 == CardType.Odin || type2 == CardType.Odin)
		{
			foreach (CardTemplate card in odinStartingCards)
			{
				AddCardToDecklist(card);
			}
		}
        if (type1 == CardType.Micki || type2 == CardType.Micki)
        {
            foreach (CardTemplate card in mickiStartingCards)
            {
                AddCardToDecklist(card);
            }
        }
        if (type1 == CardType.Reaper || type2 == CardType.Reaper)
        {
            foreach (CardTemplate card in reaperStartingCards)
            {
                AddCardToDecklist(card);
            }
        }
        if (type1 == CardType.Anubis || type2 == CardType.Anubis)
        {
            foreach (CardTemplate card in anubisStartingCards)
            {
                AddCardToDecklist(card);
            }
        }
        if (type1 == CardType.Fates || type2 == CardType.Fates)
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
}
