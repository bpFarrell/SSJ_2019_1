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
    private int _cost;
    public int cost { get{return _cost;} private set{_cost = value; } }
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
        private set { _sprite = value; }
    }
    private static CardTimeObject _projectilePrefabDefault;
    public static CardTimeObject projectilePrefabDefault {
        get {
            return _projectilePrefabDefault==null ? _projectilePrefabDefault = Resources.Load<CardTimeObject>("CardProjectileBase.prefab") : _projectilePrefabDefault; 
        }
    }
    [SerializeField]
    private CardTimeObject _projectilePrefab;
    public CardTimeObject projectilePrefab {
        get {
            if (_projectilePrefab == null)
                _projectilePrefab = Resources.Load<CardTimeObject>(assetName + ".prefab");
            return _projectilePrefab==null ? projectilePrefabDefault : _projectilePrefab;
        }
        private set { _projectilePrefab = value; }
    }
    public string jsonString;

    private JSONNode _json = null;
    public JSONNode json { get { return _json ? _json : _json = JSON.Parse(jsonString); } private set { _json = value; } }

    public void FromJSON(JSONNode json) {
        name = json["name"];
        cost = json["cost"].AsInt;
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
        jsonString = json.ToString();
    }

    public CardDefinition Copy {
        get {
            CardDefinition final = new CardDefinition();
            final.name = name;
            final.cost = cost;
            final.type = type;
            final.description = description;
            final.flavor = flavor;
            final.assetName = assetName;
            final.sprite = sprite;
            final.projectilePrefab = projectilePrefab;
            final.jsonString = jsonString;
            final.json = json;
            return final;
        }
    }
}
