using UnityEngine;

public class UIManager : UISingletonBehaviour<UIManager>
{
    [SerializeField]
    private UIMovementBar movementRef;
    [SerializeField]
    private UISkillBar skillRef;
    [SerializeField]
    private UIHandManager handRef;

    public static UIMovementBar movementManager { get { return instance.movementRef; } }
    public static UISkillBar skillManager { get { return instance.skillRef; } }
    public static UIHandManager handManager { get { return instance.handRef; } }
    public static CardEffectTurnManager _CETurnManager;
    public static CardEffectTurnManager CETurnManager { get {
            return (_CETurnManager == null) ? _CETurnManager = new CardEffectTurnManager() : _CETurnManager;
        }
    }

    [Space(10f)]
    public ColorDefine powerPallet;
    public ColorDefine movementPallet;
    public ColorDefine utilityPallet;

    public static float turnTime {
        get { return GameManager.time % 1; }
    }

    private void Awake() {
        if (movementRef == null) Debug.LogError("UIManager is missing a UIMovementBar reference");
        if (skillRef == null) Debug.LogError("UIManager is missing a UISkillBar reference");

        CETurnManager.Init();
        movementRef.Init();
        skillRef.Init();
        handRef.Init();
    }

    private void OnDisable() {
        CETurnManager.Cleanup();
        movementRef.Cleanup();
        skillRef.Cleanup();
        handRef.Cleanup();
    }

    private void Update() {
        CETurnManager.Process();
        movementRef.Process();
        skillRef.Process();
        handRef.Process();
    }
}
