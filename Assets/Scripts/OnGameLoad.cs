using UnityEngine;

public class OnGameLoad : MonoBehaviour
{
	private void Update()
	{
		if(Game.HasInstance)
		{
			Game.Instance.LoadLevel();
			this.enabled = false;
		}
	}
}
