using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneLogic : MonoBehaviour
{
    MeshRenderer mr;
    bool isHurting;
    private void OnEnable() {
        GameManager.instance.OnTurnComplete += TryCleanup;
        mr = GetComponent<MeshRenderer>();
    }
    private void OnDisable() {
        GameManager.instance.OnTurnComplete -= TryCleanup;
    }
    void TryCleanup() {
        if (isHurting) {
            Destroy(gameObject);
        } else {
            TurnOnThePain();
        }
    }
    void TurnOnThePain() {
        mr.material = new Material(mr.material);
        mr.material.SetFloat("_Alpha", 0.95f);
        isHurting = true;
        BoxCollider bc = gameObject.AddComponent<BoxCollider>();
        bc.isTrigger = true;
    }
}
