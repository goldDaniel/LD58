using System.Collections.Generic;

public class Game : MonoSingleton<Game>
{
	public List<Enemy> activeEnemies = new();

	private Enemy selectedEnemy = null;

	public override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this);
	}

	public void SelectEnemy(Enemy enemy)
	{
		if (UIController.Instance.IsSelectedCard(null))
		{
			DeselectEnemy(enemy);
			return;
		}

		if (selectedEnemy == enemy)
			return;

		if(enemy != null)
            enemy.SetHighlight(true);

        selectedEnemy = enemy;
    }

    public void DeselectEnemy(Enemy enemy)
    {
		if (selectedEnemy == enemy)
		{
            selectedEnemy = null;
			enemy.SetHighlight(false);
        }
    }

    public bool AttackEnemyWith(Card card)
	{
		if (selectedEnemy != null)
		{
			selectedEnemy.ApplyEffects(card);
			return true;
		}

		return false;
	}
}