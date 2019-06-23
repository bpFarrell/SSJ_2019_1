using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : UISingletonBehaviour<UIManager>
{
    [SerializeField]
    private UIMovementBar movementBar;
    [SerializeField]
    private UISkillBar skillBar;

    [Space(10f)]
    public ColorDefine powerPallet;
    public ColorDefine movementPallet;
    public ColorDefine utilityPallet;

    public static float turnTime {
        get { return GameManager.time % 1; }
    }

    private void Awake() {
        if (movementBar == null) Debug.LogError("UIManager is missing a UIMovementBar reference");
        if (skillBar == null) Debug.LogError("UIManager is missing a UISkillBar reference");

        movementBar.Init();
        skillBar.Init();
    }

    private void OnDisable() {
        movementBar.Cleanup();
        skillBar.Cleanup();
    }

    private void Update() {
        movementBar.process();
        skillBar.process();
    }
}
