using SimpleJSON;
using UnityEngine;

public class CardDefinition : ScriptableObject
{
    public enum TYPE
    {
        POWER,
        MOVEMENT,
        UTILITY
    }
    public new string name { get; private set; }
    public TYPE type { get; private set; }
    public string description { get; private set; }
    public string flavor { get; private set; }
    public string assetName { get; private set; }

    public void FromJSON(JSONNode json)
    {
        name = json["name"];
        string type = json["type"];
        switch (type)
        {
            case "P":
                this.type = TYPE.POWER;
                break;
            case "M":
                this.type = TYPE.MOVEMENT;
                break;
            case "U":
                this.type = TYPE.UTILITY;
                break;
        }
        description = json["description"];
        flavor = json["flavor"];
        assetName = json["resourceName"];
    }
}
