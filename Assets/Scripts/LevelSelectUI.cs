using Assets.Scripts;
using System;
using System.Collections.Generic;
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

		SetUnlockedLevels(GameProgress.Instance);
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

	public void SetUnlockedLevels(GameProgress progress)
	{
		for (int i = 0; i < progress.anubisLevels.Count - 1; ++i)
		{
			var unlocked = progress.completedLevels[progress.anubisLevels[i]];
			anubisButtons[i + 1].interactable = unlocked;
		}
		for (int i = 0; i < progress.fateLevels.Count - 1; ++i)
		{
			var unlocked = progress.completedLevels[progress.fateLevels[i]];
			fateButtons[i + 1].interactable = unlocked;
		}
		for (int i = 0; i < progress.mickiLevels.Count - 1; ++i)
		{
			var unlocked = progress.completedLevels[progress.mickiLevels[i]];
			mickiButtons[i + 1].interactable = unlocked;
		}
		for (int i = 0; i < progress.odinLevels.Count - 1; ++i)
		{
			var unlocked = progress.completedLevels[progress.odinLevels[i]];
			odinButtons[i + 1].interactable = unlocked;
		}
		for (int i = 0; i < progress.reaperLevels.Count - 1; ++i)
		{
			var unlocked = progress.completedLevels[progress.reaperLevels[i]];
			reaperButtons[i + 1].interactable = unlocked;
		}
	}
}
