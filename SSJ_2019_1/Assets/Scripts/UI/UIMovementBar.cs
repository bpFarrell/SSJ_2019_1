using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovementBar : UIMonoBehaviour {
    public Image bar;
    public GameManager g { get { return GameManager.instance; } }
    public float lastSegmentTurnPercent {
        get {
            return ((g.player.poses.Count * g.player.poseSaveIntervals)
                 / g.turnLength) - (g.turnNumber - 1);
        }
    }
    private float animT = 0f;
    private float animScalar = 1f;
    private float segmentTurnValue = 0f;

    internal void Init() {
        if (bar == null) Debug.LogError("UIMovementBar is missing an image reference");
    }

    internal void Cleanup() {
        
    }

    internal void Process() {
        animT += Time.deltaTime * animScalar;
        if (animT > 1f) animT -= 1f;
        segmentTurnValue = Mathf.Lerp(segmentTurnValue, lastSegmentTurnPercent, 0.2f);
        bar.material.SetVector("_Meta", new Vector4(animT, segmentTurnValue, 32.14f, GameManager.instance.percentThroughTurn));
    }
}
