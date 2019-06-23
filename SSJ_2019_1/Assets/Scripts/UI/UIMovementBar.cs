using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovementBar : UIMonoBehaviour {
    public RectTransform moveRect;

    internal void Init() {
        if (moveRect == null) Debug.LogError("UIMovementBar is missing a rect reference");
    }

    internal void Cleanup() {
        
    }

    internal void process() {
        moveRect.anchorMax = new Vector2(UIManager.turnTime, moveRect.anchorMax.y);
    }
}
