using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "New Card", order = 1)]
public class CardTemplate : ScriptableObject
{
    //Card Properties
    public string CardDescription;

    //Cost
    public bool TurnDelay = false;
    public int EssenceCost = -1;
    public int SelfDamage = -1;

    //Enemy Effect
    public bool TargerAllEnemies = false;
    public bool RandomEnemy = false;
    public int MinDamage = -1;
    public int MaxDamage = -1;
    public int Doomed = -1;
    public bool Jinxed = false;
    public bool FateSealed = false;
    public bool Confuse = false;
    public int Souls = -1;
    public int Curse = -1;

    //Self Effect
    public int Heal = -1;
    public int Block = -1;
    public bool GetLucky = false; //reroll damage, take highest result
    public int Strength = -1; //add value to future attacks
    public int EssenceGain = -1;
    public int MultHit = -1;
    public bool Power = false;//every 3 cards played, take 3 damage, gain 1 essence
}