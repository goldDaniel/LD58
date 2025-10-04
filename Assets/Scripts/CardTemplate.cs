using UnityEngine;

public enum CardType { None, Anubis, Micki, Odin, Reaper, Fates };

[CreateAssetMenu(fileName = "NewCard", menuName = "New Card", order = 1)]
public class CardTemplate : ScriptableObject
{
    //Card Properties
    public string CardDescription;
    public CardType Type = CardType.None;
	public Sprite cardFront;

    //Cost
    public int EssenceCost = -1;
    public int SelfDamage = -1;

    //Enemy Effect
    public bool TargetAllEnemies = false;
    public bool RandomEnemy = false;
    public int MinDamage = -1;
    public int MaxDamage = -1;
    public int BloodyStrike = -1; //take % health damage, do amount to enemy
    public int Doomed = -1; //if Doom >= 3, enemy instantly dies
    public bool Jinxed = false; //enemy has a 50% chance to miss
    public bool FateSealed = false; //enemy goes to next attack in cycle
    public bool Confuse = false; //enemy has a chance to hit itself
    public int Souls = -1;
    public int Curse = -1; //DoT
    public int DeathChance = -1; //when triggered, this per cent has a chance to instantly kill hit enemies
    public int Weak = -1; //reduces enemy damage by 1

    //Self Effect
    public int Draw = -1;
    public int Heal = -1;
    public int Block = -1;
    public bool GetLucky = false; //reroll damage, take highest result
    public int Strength = -1; //add value to future attacks
    public int EssenceGain = -1;
    public int MultHit = -1;
    public int doubleDamageHits = -1;
    public bool Power = false; //every 3 cards played, take 3 damage, gain 1 essence
    public bool Sacrifice = false; //self-inflicted damage is done to all enemies
    public bool RepeatAllNext = false; //cards trigger twice next turn
    public bool Foretell = false; //draw 1 and reduce its cost to 0
    public bool CurseEachPlay = false;
}