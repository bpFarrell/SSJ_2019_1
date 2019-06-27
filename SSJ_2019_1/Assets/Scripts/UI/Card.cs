using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : UIMonoBehaviour, ISelectHandler, IDeselectHandler, ICancelHandler, ISubmitHandler {
    private CardDefinition definition;
    public Selectable selectable;
    public readonly Color PowerColor = new Color();

    public int handIndex = -1;
    public float xPivot = 0.5f;

    [Space(10f)]
    // Base
    [SerializeField]
    private Image baseBackground;
    [SerializeField]
    private Image innerBackground;
    // Header
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Image costBackground;
    [SerializeField]
    private Text costText;

    [Space(10f)]
    // Image
    [SerializeField]
    private Image image;

    [Space(10f)]
    // Description
    [SerializeField]
    private Text descText;

    private void Awake() { }

    public void OnSelect(BaseEventData eventData) {
        UIManager.handManager.hidden = false;
        UIManager.handManager.selectionIndex = handIndex;
    }

    public void OnDeselect(BaseEventData eventData) {
        // Do something?
    }
    public void OnCancel(BaseEventData eventData) {
        EventSystem.current.SetSelectedGameObject(UICenter.instance.gameObject, eventData);
    }
    public void OnSubmit(BaseEventData eventData) {
        Debug.Log("Selected Card: " + definition.name);
        UIHandManager.CardTriggered(definition);
    }

    public void Load(CardDefinition def) {
        definition = def;
        Navigation nav = selectable.navigation;
        nav.selectOnUp = UICenter.instance.selectable;
        selectable.navigation = nav;
        apply();
    }

    public void SetLeft(Selectable s) {
        Navigation nav = selectable.navigation;
        nav.selectOnLeft = s;
        selectable.navigation = nav;
    }
    public void SetRight(Selectable s)
    {
        Navigation nav = selectable.navigation;
        nav.selectOnRight = s;
        selectable.navigation = nav;
    }

    public void select(bool selected) {
        targetPose = selected ? displayPose : handPose;
    }

    private void apply() {
        if (definition == null) return;
        titleText.text = definition.name;
        costText.text = definition.cost.ToString();
        descText.text = definition.description;

        switch (definition.type) {
            case CardDefinition.TYPE.POWER:
                baseBackground.color = UIManager.instance.powerPallet.secondary;
                innerBackground.color = UIManager.instance.powerPallet.primary;
                costBackground.color = UIManager.instance.powerPallet.hue;
                break;
            case CardDefinition.TYPE.MOVEMENT:
                baseBackground.color = UIManager.instance.movementPallet.secondary;
                innerBackground.color = UIManager.instance.movementPallet.primary;
                costBackground.color = UIManager.instance.movementPallet.hue;
                break;
            case CardDefinition.TYPE.UTILITY:
                baseBackground.color = UIManager.instance.utilityPallet.secondary;
                innerBackground.color = UIManager.instance.utilityPallet.primary;
                costBackground.color = UIManager.instance.utilityPallet.hue;
                break;
        }
    }

    // Movement
    public struct Pose {
        public Vector2 position;
        public Quaternion rotation;
        public Vector3 scale;
        public Vector2 pivot;

        public static Pose Empty() {
            Pose t;
            t.position = Vector2.zero;
            t.rotation = Quaternion.identity;
            t.scale = Vector2.one;
            t.pivot = new Vector2(0.5f, 0.5f);
            return t;
        }
        public Pose(RectTransform t) {
            position = t.anchoredPosition;
            rotation = t.localRotation;
            scale = t.localScale;
            pivot = t.pivot;
        }
        public static Pose Lerp(Pose a, Pose b, float t) {
            Pose final = b;
            if ((b.pivot - a.pivot).magnitude >= 0.01f) {
                final.pivot = Vector2.Lerp(a.pivot, b.pivot, t);
            }
            if ((b.position - a.position).magnitude >= 0.01f) {
                final.position = Vector3.Lerp(a.position, b.position, t);
            }
            if (b.rotation != a.rotation) {
                final.rotation = Quaternion.Lerp(a.rotation, b.rotation, t);
            }
            if ((b.scale - a.scale).magnitude >= 0.02f) {
                final.scale = Vector3.Lerp(a.scale, b.scale, t);
            }
            return final;
        }
        public void IntoRectTransform(ref RectTransform rect) {
            rect.anchoredPosition = position;
            rect.localRotation = rotation;
            rect.localScale = scale;
            rect.pivot = pivot;
        }
        public bool equal(RectTransform rect) {
            return rect.pivot == pivot &&
                   rect.anchoredPosition == position &&
                   rect.localRotation == rotation &&
                   rect.localScale == scale;
        }
    }

    public Pose targetPose = Pose.Empty();
    private Pose _handPose = Pose.Empty();
    public Pose handPose {
        get { return _handPose; }
        set {
            _handPose = value;
            displayPose.position.x = _handPose.position.x;
            displayPose.position.y = 130f;
            displayPose.rotation = Quaternion.identity;
            displayPose.scale = _handPose.scale * 1.3f;
        }
    }
    public Pose displayPose = Pose.Empty();

    public void Update(){
        if (targetPose.equal(rectTransform)) return;

        Pose current = new Pose(rectTransform);

        current = Pose.Lerp(current, targetPose, 6.0f * Time.deltaTime);

        rectTransform.anchoredPosition = current.position;
        rectTransform.localRotation = current.rotation;
        rectTransform.localScale = current.scale;
        rectTransform.pivot = current.pivot;
    }
}