
using UnityEngine;

public class EnemyArtContainer : MonoSingleton<EnemyArtContainer>
{
	// Cursed syntax

	public Sprite Spirit;
	public Sprite Mummy;
	public Sprite Warrior;
	public Sprite Chiefton;
	public Sprite Skeleton;
	public Sprite MegaSkelebro;
	public Sprite Soldier;
	public Sprite Knight;
	public Sprite Priest;
	public Sprite Demigod;

	public Sprite FindSprite(EnemyModel model)
	{
		switch (model)
		{
			case EnemyModel.Spirit:
				return Spirit;
			case EnemyModel.Mummy:
				return Mummy;
			case EnemyModel.Warrior:
				return Warrior;
			case EnemyModel.Chiefton:
				return Chiefton;
			case EnemyModel.Skeleton:
				return Skeleton;
			case EnemyModel.MegaSkelebro:
				return MegaSkelebro;
			case EnemyModel.Soldier:
				return Soldier;
			case EnemyModel.Knight:
				return Knight;
			case EnemyModel.Priest:
				return Priest;
			case EnemyModel.Demigod:
				return Demigod;
			default:
				return null;
		}
	}
}