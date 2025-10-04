
using Assets.Scripts;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class GameProgress : MonoSingleton<GameProgress>
{
	public LevelTemplate selectedLevel = null;

	public Dictionary<LevelTemplate, bool> completedLevels = new();

	public List<LevelTemplate> anubisLevels;
	public List<LevelTemplate> fateLevels;
	public List<LevelTemplate> mickiLevels;
	public List<LevelTemplate> odinLevels;
	public List<LevelTemplate> reaperLevels;

	public override void Awake()
	{
		base.Awake();

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
}
