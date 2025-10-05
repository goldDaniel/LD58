using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
	public List<Button> anubisButtons;
	public List<Button> fateButtons;
	public List<Button> mickiButtons;
	public List<Button> odinButtons;
	public List<Button> reaperButtons;

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

	public void SetUnlockedLevels(List<Button> buttons, List<LevelTemplate> levels)
	{
		for (int i = 0; i < levels.Count - 1; ++i)
		{
			var unlocked = GameProgress.Instance.completedLevels[levels[i]];
			buttons[i + 1].interactable = unlocked;
		}
	}
}
