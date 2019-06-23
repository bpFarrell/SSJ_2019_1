using System.Collections.Generic;
using UnityEngine;

public class UISkillBar : MonoBehaviour {
    private const float TOTALWIDTH = 1440;
    private const int TOTALSLOTS = 10;
    private static float costCoefficient { get { return TOTALWIDTH / TOTALSLOTS; } }

    private static float GetWidthFromCost(int cost) {
        return costCoefficient * cost;
    }

    private List<int> activeCardIDs = new List<int>();

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    internal void Init() {

    }

    internal void Cleanup() {

    }

    internal void process() {
        
    }

    private void nextAvailableSlot(int cost) {

    }
}