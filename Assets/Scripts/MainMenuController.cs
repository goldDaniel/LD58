using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	public RectTransform mainMenuPanel;
	public RectTransform howToPlayPanel;
 
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
}
