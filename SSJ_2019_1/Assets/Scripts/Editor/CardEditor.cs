using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor {
    private CardDefinition def;

    public override void OnInspectorGUI() {
        Card script = (Card)target;
        DrawDefaultInspector();

        EditorGUILayout.Space();
        def = EditorGUILayout.ObjectField("Definition: ", def, typeof(CardDefinition), true) as CardDefinition;
        EditorGUI.BeginDisabledGroup(def == null);
        if (GUILayout.Button("load")) {
            script.load(def);
        }
        EditorGUI.EndDisabledGroup();
    }
}
