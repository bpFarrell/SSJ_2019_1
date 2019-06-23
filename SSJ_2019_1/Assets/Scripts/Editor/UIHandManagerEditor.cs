using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIHandManager))]
public class UIHandManagerEditor : Editor {
    private CardDefinition def;

    public override void OnInspectorGUI() {
        UIHandManager script = (UIHandManager)target;
        DrawDefaultInspector();

        EditorGUILayout.Space();
        def = EditorGUILayout.ObjectField("Definition: ", def, typeof(CardDefinition), true) as CardDefinition;
        EditorGUI.BeginDisabledGroup(def == null);
        if (GUILayout.Button("Add")) {
            script.ReceiveCard(def);
        }
        EditorGUI.EndDisabledGroup();
    }
}
