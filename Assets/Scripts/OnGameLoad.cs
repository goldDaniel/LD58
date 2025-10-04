using UnityEngine;

public class OnGameLoad : MonoBehaviour
{
	private void LateUpdate()
	{
		if(Game.HasInstance)
		{
			this.enabled = false;
		}
	}
}
