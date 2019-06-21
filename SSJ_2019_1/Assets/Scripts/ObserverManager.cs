using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class ObserverManager : MonoBehaviour {
    public static ObserverManager instance;
    public IDictionary<int, IObserver> registry = new Dictionary<int, IObserver>();
    public List<JSONObject> history = new List<JSONObject>();
    public int counter = 0;
    public int frameIndex = 0;
    public bool apply;
    public bool capture;
    private void Awake() {
        instance = this;
    }
    public void Update() {
        if (apply) {
            apply = false;
            GetFrame(frameIndex);
        }
        if (capture) {
            capture = false;
            TakeFrame();
        }
    }
    public static void Register(IObserver ob) {
        instance.registry.Add(instance.counter, ob);
        ob.id = instance.counter;
        instance.counter++;
    }
    public static void Unregister(IObserver ob) {
        instance.registry.Remove(ob.id);
    }
    public void TakeFrame() {
        JSONObject json = new JSONObject();
        foreach(var ob in registry) {
            json["frame"][-1] = ob.Value.GetSnapShot();
        }
        history.Add(json);
    }
    public void GetFrame(int index) {
        JSONObject json = history[index];
        for(int x= 0; x < json["frame"].Count; x++) {
            IObserver ob = registry[json["frame"][x]["id"].AsInt];
            ob.ApplySnapShot(json["frame"][x]as JSONObject);
        }
    }
}
