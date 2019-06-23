using SimpleJSON;
using UnityEngine;

[CreateAssetMenuAttribute]
public class CardDefinition : ScriptableObject {
    public enum TYPE
    {
        POWER,
        MOVEMENT,
        UTILITY
    }
    [SerializeField]
    private string _name;
    public new string name { get{ return _name; } private set{_name = value; } }
    [SerializeField]
    private string _cost;
    public string cost { get{return _cost;} private set{_cost = value; } }
    [SerializeField]
    private TYPE _type;
    public TYPE type { get{return _type;} private set{_type = value; } }
    [SerializeField]
    private string _description;
    public string description { get{return _description;} private set{_description = value; } }
    [SerializeField]
    private string _flavor;
    public string flavor { get{return _flavor;} private set{_flavor = value; } }
    [SerializeField]
    private string _assetName;
    public string assetName { get{return _assetName;} private set{_assetName = value; } }
    private Sprite _sprite;
    public Sprite sprite {
        get {
            if (_sprite == null)
                _sprite = Resources.Load<Sprite>(assetName + ".png");
            return _sprite;
        }
    }

    public void FromJSON(JSONNode json) {
        name = json["name"];
        cost = json["cost"];
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
