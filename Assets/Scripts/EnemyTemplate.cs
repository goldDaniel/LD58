using NUnit.Framework;
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
    Solider,
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

    //Stats
    public int MaxHealth = -1;
    public int Block = -1;

    public EnemyAttack Attack = new EnemyAttack();
}

[Serializable]
public class EnemyAttack
{
    public List<EnemyAttackTemplate> Attacks = new List<EnemyAttackTemplate>();
}
