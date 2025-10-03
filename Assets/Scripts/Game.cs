public class Game : MonoSingleton<Game>
{
	public override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this);
	}
}