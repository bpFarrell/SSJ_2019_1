using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class PhysicsObserver : Observer
{
    Rigidbody rb;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    public override void ApplySnapShot(JSONObject json) {
        base.ApplySnapShot(json);
        rb.velocity = new Vector3(
        json["vel"][-1].AsFloat,
        json["vel"][-1].AsFloat,
        json["vel"][-1].AsFloat);


        rb.angularVelocity = new Vector3(
        json["angleVel"][-1].AsFloat,
        json["angleVel"][-1].AsFloat,
        json["angleVel"][-1].AsFloat);

    }
    public override JSONObject GetSnapShot() {
        JSONObject json= base.GetSnapShot();
        json["vel"][-1] = rb.velocity.x;
        json["vel"][-1] = rb.velocity.y;
        json["vel"][-1] = rb.velocity.z;

        json["angleVel"][-1] = rb.angularVelocity.x;
        json["angleVel"][-1] = rb.angularVelocity.y;
        json["angleVel"][-1] = rb.angularVelocity.z; 
        return json;
    }
}
