
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "Enemy Attack", order = 1)]
public class EnemyAttackTemplate : ScriptableObject
{
	public string Description = string.Empty;

	public int Damage = -1;
	public int Heal = -1;
	public int Block = -1;
	public int Curse = -1;
	public int NumberOfAttacks = -1;
	public int Strength = -1;

	public bool ClearNegative = false; //clears all negative effects on enemy
	public bool SpawnEnemy = false;
	public bool MassBonus = false; //gives extra damage based on the number of Enemies
	public bool BlockBonus = false; //gives extra damage based on Enemy block
	public bool MissingHealthBonus = false; //gives extra damage based on Enemy missing health
	public bool ApplyLethergy = false; //reduce players draw by one

	public bool TargetAllEnemies = false;
	public bool TargetAllOtherEnemies = false;
	public bool TargetRandomEnemy = false;
	public bool TargetSelf = false;
}