using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : UIMonoBehaviour{
    private CardDefinition definition;
    public readonly Color PowerColor = new Color();

    public int handIndex = -1;

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

    public void load(CardDefinition def) {
        definition = def;
        apply();
    }

    private void apply() {
        if (definition == null) return;
        titleText.text = definition.name;
        costText.text = definition.cost;
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
}