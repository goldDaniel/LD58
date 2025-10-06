using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	public RectTransform mainMenuPanel;
	public RectTransform howToPlayPanel;

	public RectTransform storyPanel;

	public CanvasGroup panel0;
	public CanvasGroup panel1;
	public CanvasGroup panel2;

	public void Start()
	{
		AudioManager.Instance.PlayMusicCrossfade("CombatMusic");
	}

	public void OnPlayPressed()
	{
		if (GameProgress.Instance.hasCompletedTutorial)
		{
			SceneManager.LoadScene("Level Select");
		}
		else
		{
			mainMenuPanel.gameObject.SetActive(false);
			howToPlayPanel.gameObject.SetActive(true);
		}
	}

	public void OnTutorialPlayPressed()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void StoryPressed()
	{
		StartCoroutine(AnimatePanels());
	}

	public void OnReturnToMenuPressed()
	{
		panel0.alpha = 0;
		panel1.alpha = 0;
		panel2.alpha = 0;
		storyPanel.gameObject.SetActive(false);
	}

	IEnumerator AnimatePanels()
	{
		panel0.alpha = 0;
		panel1.alpha = 0;
		panel2.alpha = 0;
		storyPanel.gameObject.SetActive(true);

		yield return new WaitForSecondsRealtime(0.1f);

		yield return FadeInPanel(panel0);
		yield return new WaitForSeconds(1f);
		yield return FadeInPanel(panel1);
		yield return new WaitForSeconds(1f);
		yield return FadeInPanel(panel2);
	}

	IEnumerator FadeInPanel(CanvasGroup panel)
	{
		float timer = 0;
		float time = 2f;
		while (timer < time)
		{
			panel.alpha = Mathf.Pow(timer / time, 2.2f);
			timer += Time.deltaTime;
			yield return null;
		}
		panel.alpha = 1;
	}
}
