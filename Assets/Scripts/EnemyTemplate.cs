using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy", order = 1)]
public class EnemyTemplate : ScriptableObject
{
    //Enemy Properties
    public string EnemyDescription;

    //Stats
    public int MaxHealth = -1;
    public int Block = -1;
    public int CurseStacks = -1;
    public int Strength = -1;

    public EnemyAttack Attack = new EnemyAttack();
}

[Serializable]
public class EnemyAttack
{
    public List<EnemyAttackTemplate> Attacks = new List<EnemyAttackTemplate>();
}
