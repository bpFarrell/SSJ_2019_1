using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDeathAnim : TimeObject {
    public float kill = 0;
    public float startScale = 1;
    public Vector3 startPoint;
    Material mat;
    private void Awake() {
        mat = GetComponent<MeshRenderer>().material;
        startScale = transform.localScale.x;
        startPoint = transform.position;
    }

    public void Init(IEvaluable parent) {
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return parent.eval(parentAgeAtBirth)+Vector3.forward*-0.01f; };
        
        scheduledDeathTime = 1;
    }
    private void Update() {
        float tScaled = Mathf.Clamp(t * 5,0,2);
        mat.SetFloat("_Kill", tScaled);
        transform.localScale = Vector3.one * (tScaled * 2 + 1 ) * startScale;
        TimeUpdate();
    }
    public override void BeforeBirth() {
        Destroy(gameObject);
    }
}
