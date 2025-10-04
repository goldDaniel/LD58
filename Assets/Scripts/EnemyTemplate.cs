
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyModel
{
    None,
    Spirit,
    Mummy,
    Warrior,
    Chiefton,
    Skeleton,
    MegaSkelebro,
    Soldier,
    Knight,
    Priest,
    Demigod
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy", order = 1)]
public class EnemyTemplate : ScriptableObject
{
    //Enemy Properties
    public string EnemyDescription = string.Empty;
    public EnemyModel EnemyModel = EnemyModel.None;

	public Sprite sprite;

    //Stats
    public int MaxHealth = -1;
    public int Block = -1;

    public EnemyAttack AttackList = new EnemyAttack();
}

[Serializable]
public class EnemyAttack
{
    public List<EnemyAttackTemplate> Attacks = new List<EnemyAttackTemplate>();
}
