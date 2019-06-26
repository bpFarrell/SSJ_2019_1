using System.Collections;
using System.Collections.Generic;

public class CardEffect{
    public CardDefinition def;
    // 
    public enum CardType {
        projectile, // Creates a projectile
        hand, // Modifies cards in the hand ( Limit/Buff card effects, Change incomming effect types etc)
        player, // Targets the player and their stats
        none // Unassigned/Uninitialized
    }
    // type of card effect
    public CardType cardType;

    // Shot Stats
    public enum ShotType {
        none,
        simple, // 
        explosive,
        pierce
    }
    // projectile
    public int shotCount = 0;
    public float shotFrequency = 0f;
    public int shotSpread = 0;
    public ShotType shotType = ShotType.none;
    
    // Player Stats
    // 

    public void Load(CardDefinition def) {
    }
}

public class ShotEffect : CardEffect {

}

public class BuffEffect : CardEffect {

}

public class ModifierEffect : CardEffect {

}
