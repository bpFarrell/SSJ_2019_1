using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UISkillBar))]
public class UISkillBarEditor : Editor {
    public int cost = 1;
    public int removeIndex = 1;

    public override void OnInspectorGUI()
    {
        UISkillBar script = (UISkillBar)target;
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(true);
        cost = EditorGUILayout.IntField("Cost", cost);
        if (GUILayout.Button("Find Space")) {
            int index = script.nextAvailableIndex(cost);
            for (int i = 0; i < cost; i++) {
                //script.tabList[index + i] = new UISkillBar.example();
            }
        }
        removeIndex = EditorGUILayout.IntField("remove at", removeIndex);
        if (GUILayout.Button("Remove")) {
            script.tabList[removeIndex] = null;
        }
        EditorGUI.EndDisabledGroup();
    }
}