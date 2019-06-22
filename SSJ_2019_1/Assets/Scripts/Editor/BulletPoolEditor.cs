using UnityEditor;


[CustomEditor(typeof(BulletPool))]
public class BulletPoolEditor : Editor
{
    public override void OnInspectorGUI() {
        BulletPool script = (BulletPool)target;

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("time", script.t);
        EditorGUILayout.FloatField("spawn time", script.spawnTime);
        EditorGUILayout.FloatField("death time", script.deathTime);
        EditorGUI.EndDisabledGroup();

        DrawDefaultInspector();
    }
}
