using UnityEditor;

[CustomEditor(typeof(BulletPool)), CanEditMultipleObjects]
public class BulletPoolEditor : Editor
{
    private SerializedProperty m_t;
    private SerializedProperty m_spawnTime;
    private SerializedProperty m_deathTime;

    private void OnEnable()
    {
        m_t = serializedObject.FindProperty("t");
        m_spawnTime = serializedObject.FindProperty("spawnTime");
        m_deathTime = serializedObject.FindProperty("deathTime");
    }

    public override void OnInspectorGUI() {
        BulletPool script = (BulletPool)target;

        EditorGUI.BeginDisabledGroup(true);
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_t);
        EditorGUILayout.PropertyField(m_spawnTime);
        EditorGUILayout.PropertyField(m_deathTime);
        serializedObject.ApplyModifiedProperties();
        EditorGUI.EndDisabledGroup();

        DrawDefaultInspector();
    }
}
