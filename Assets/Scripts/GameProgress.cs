
using Assets.Scripts;
using System;
using System.Collections.Generic;
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

	public void SelectLevel(LevelTemplate level)
	{
		selectedLevel = level;
		SceneManager.LoadScene("GameScene");
	}

	internal void CompleteCurrentLevel()
	{
		if(selectedLevel != null)
			completedLevels[selectedLevel] = true;
	}
}
