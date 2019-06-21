using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class Observer : MonoBehaviour, IObserver
{
    public int id { get; set; }

    public virtual JSONObject GetSnapShot(){
        JSONObject json = new JSONObject();
        json["id"].AsInt = id;
        json["pos"][-1].AsFloat = transform.position.x;
        json["pos"][-1].AsFloat = transform.position.y;
        json["pos"][-1].AsFloat = transform.position.z;
        json["rot"][-1].AsFloat = transform.eulerAngles.x;
        json["rot"][-1].AsFloat = transform.eulerAngles.y;
        json["rot"][-1].AsFloat = transform.eulerAngles.z;
        return json;
    }

    public virtual void ApplySnapShot(JSONObject json) {
        transform.position = new Vector3(
            json["pos"][0].AsFloat,
            json["pos"][1].AsFloat,
            json["pos"][2].AsFloat);
        transform.eulerAngles = new Vector3(
            json["rot"][0].AsFloat,
            json["rot"][1].AsFloat,
            json["rot"][2].AsFloat);
    }
    void OnEnable() {
        ObserverManager.Register(this);
    }
    void OnDisable() {
        ObserverManager.Unregister(this);
    }
}
