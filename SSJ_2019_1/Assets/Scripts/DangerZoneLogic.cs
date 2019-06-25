using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneLogic : MonoBehaviour
{
    private void OnEnable() {
        GameManager.instance.OnTurnComplete += CleanUp;
    }
    private void OnDisable() {
        GameManager.instance.OnTurnComplete -= CleanUp;
    }
    void CleanUp() {
        Destroy(gameObject);
    }
}
