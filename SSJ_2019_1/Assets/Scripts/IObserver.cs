using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public interface IObserver
{
    int id { get; set;}
    JSONObject GetSnapShot();
    void ApplySnapShot(JSONObject json);

}
