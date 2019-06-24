using UnityEditor;

[CustomEditor(typeof(BulletPool)), CanEditMultipleObjects]
public class BulletPoolEditor : Editor
{

    private void OnEnable() { }

    public override void OnInspectorGUI() {
        //BulletPool[] scripts = (BulletPool[])targets;

        EditorGUI.BeginDisabledGroup(true);
        foreach (BulletPool script in targets) {
            EditorGUILayout.FloatField("time", script.t);
            EditorGUILayout.FloatField("spawn time", script.spawnTime);
            EditorGUILayout.FloatField("death time", script.scheduledDeathTime);
            EditorGUILayout.FloatField("actual death time", script.actualdDeathTime);
        }
        EditorGUI.EndDisabledGroup();

        DrawDefaultInspector();
    }
}
