using System.Collections;

public class Player
{
    public int Strength = 0;
    public int MaxHealth = 0;
    public int CurrentHealth = 0;
    public int MaxEssence = 0;
    public int CurrentEssence = 0;
    public int Block = 0;
    public int RepeatAllNext = 0;
    public int RepeatAllCurrentTurn = 0;
    public int doubleDamageHit = 0;
    public int PactOfPower = 0;
    public int PactOfSacrifice = 0;
    public int SacrificeCounter = 0;
    public int CurseEachPlay = 0;
    public int Lucky = 0;

    public IEnumerator ApplyEffectSequence(Card card)
    {
        if (card.cardTemplate.Draw > 0)
        {
            // TODO draw cards
        }
        if (card.cardTemplate.Heal > 0)
        {
            Heal(card.cardTemplate.Heal);
        }
        if (card.cardTemplate.Block > 0)
        {
            Block += card.cardTemplate.Block;
        }
        if (card.cardTemplate.GetLucky)
        {
            Lucky += 1;
        }
        if (card.cardTemplate.Strength > 0)
        {
            Strength += card.cardTemplate.Strength;
        }
        if (card.cardTemplate.EssenceGain > 0)
        {
            CurrentEssence += card.cardTemplate.EssenceGain;
        }
        if (card.cardTemplate.Power)
        {
            PactOfPower += 1;
        }
        if (card.cardTemplate.Sacrifice)
        {
            PactOfSacrifice += 1;
        }
        if (card.cardTemplate.RepeatAllNext)
        {
            RepeatAllNext += 1;
        }
        if (card.cardTemplate.Foretell)
        {
            //TODO Draw card and reduce cost to 0
        }
        if (card.cardTemplate.CurseEachPlay)
        {
            CurseEachPlay += 1;
        }

        yield return null;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            //TODO die-
        }
    }
    public void Heal(int healing) 
    {
        CurrentHealth += healing;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
    
}