using Assets.Scripts;
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
		for (int i = 1; i < anubisButtons.Count; ++i)
			anubisButtons[i].interactable = false;
		for (int i = 1; i < fateButtons.Count; ++i)
			fateButtons[i].interactable = false;
		for (int i = 1; i < mickiButtons.Count; ++i)
			mickiButtons[i].interactable = false;
		for (int i = 1; i < odinButtons.Count; ++i)
			odinButtons[i].interactable = false;
		for (int i = 1; i < reaperButtons.Count; ++i)
			reaperButtons[i].interactable = false;

		SetUnlockedLevels(GameProgress.Instance);
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
