using SimpleJSON;
using System;
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
    public CardType cardType = CardType.none;

    // Shot Stats
    public enum ShotType {
        none,
        simple, // 
        explosive,
        pierce // Nope
    }
    // projectile
    public int duration = 0;
    public int shotCount = 0;
    public float shotSpeed = 0f;
    public float shotCountScalar = 1f;
    public int shotCountModifier = 0;
    public float shotFrequency = 0f;
    public float shotFrequencyScalar = 1f;
    public float shotFrequencyModifier = 0f;
    public float shotSpread = 0;
    public float shotSpreadScalar = 1f;
    public float shotSpreadModifier = 0;
    public float recieveDamageScalar = 1f;
    public int receiveDamageModifier = 0;
    public ShotType shotType = ShotType.none;

    // Player Stats
    public float movementScalar = 1f;
    // 

    public void Load(CardDefinition def) {
        this.def = def;
        // Meta
        duration = def.json["duration"].AsInt;
        try {
            shotType = (ShotType)Enum.Parse(typeof(ShotType), def.json["shotType"]);
        }
        catch { }
        try
        {
            cardType = (CardType)Enum.Parse(typeof(CardType), def.json["cardType"]);
        }
        catch { }
        //
        // Projectiles
        shotCount = def.json["shotCount"].AsInt;
        shotSpeed = def.json["projectileSpeed"].AsFloat;
        shotCountScalar = def.json["shotCountScalar"].AsFloat;
        shotCountModifier = def.json["shotCountModifier"].AsInt;
        shotFrequency = def.json["shotFrequency"].AsFloat;
        shotFrequencyScalar = def.json["shotFrequencyScalar"].AsFloat;
        shotFrequencyModifier = def.json["shotFrequencyModifier"].AsFloat;
        shotSpread = def.json["shotSpread"].AsFloat;
        shotSpreadScalar = def.json["shotSpreadScalar"].AsFloat;
        shotSpreadModifier = def.json["shotSpreadModifier"].AsFloat;
        //
        // Player Stats
        recieveDamageScalar = def.json["recieveDamageScalar"].AsFloat;
        receiveDamageModifier = def.json["receiveDamageModifier"].AsInt;
    }
}

public class ShotEffect : CardEffect {

}

public class BuffEffect : CardEffect {

}

public class ModifierEffect : CardEffect {

}
