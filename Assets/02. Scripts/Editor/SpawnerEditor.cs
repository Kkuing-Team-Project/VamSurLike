using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
    private SerializedProperty m_SpawnObjects;
    private SerializedProperty m_Waves;

    private void OnEnable()
    {
        m_SpawnObjects = serializedObject.FindProperty("spawnObjects");
        m_Waves = serializedObject.FindProperty("waves");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Spawner myTarget = (Spawner)target;

        myTarget.isWave = EditorGUILayout.Toggle("Is Wave", myTarget.isWave);
        if (myTarget.isWave)
        {
            EditorGUILayout.PropertyField(m_Waves.FindPropertyRelative("Array.size"), new GUIContent("Wave Count"));
            EditorGUI.indentLevel = 1;
            for (int i = 0; i < m_Waves.arraySize; ++i)
            {
                GUILayout.BeginVertical("", new GUIStyle(GUI.skin.box));
                EditorGUILayout.PropertyField(m_Waves.GetArrayElementAtIndex(i), new GUIContent("Wave " + i));
                GUILayout.EndVertical();
            }
        }
        else
        {
            EditorGUILayout.PropertyField(m_SpawnObjects.FindPropertyRelative("Array.size"), new GUIContent("Spawn Object Count"));
            EditorGUI.indentLevel = 1;
            for (int i = 0; i < m_SpawnObjects.arraySize; ++i)
            {
                GUI.color = Color.gray;
                GUILayout.BeginVertical("", new GUIStyle(GUI.skin.box));
                GUI.color = Color.white;
                EditorGUILayout.PropertyField(m_SpawnObjects.GetArrayElementAtIndex(i), new GUIContent("Spawn Object " + i));
                GUILayout.EndVertical();
            }
        }
        EditorGUI.indentLevel = 0;

        myTarget.isMax = EditorGUILayout.Toggle("Is Max", myTarget.isMax);
        if (myTarget.isMax)
        {
            myTarget.maxRangeRadius = EditorGUILayout.FloatField("Max Range Radius", myTarget.maxRangeRadius);
            myTarget.entityRadius = EditorGUILayout.FloatField("Entity Radius", myTarget.entityRadius);
        }

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            myTarget.maxPercent = 0;
            foreach (SpawnObject spawnObject in myTarget.spawnObjects)
            {
                float max = myTarget.maxPercent + spawnObject.percent;
                spawnObject.SetRealPercent(myTarget.maxPercent, max);

                myTarget.maxPercent = max;
            }

            EditorUtility.SetDirty(myTarget);
        }
    }
}

[CustomPropertyDrawer(typeof(SpawnObject))]
public class SpawnObjectDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        int startIndentLevel = EditorGUI.indentLevel;
        if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
        {
            EditorGUI.indentLevel = startIndentLevel + 1;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(property.FindPropertyRelative("prefab"), new GUIContent("Prefab"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("type"), new GUIContent("Type"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("percent"), new GUIContent("Percent"));
            EditorGUI.indentLevel = startIndentLevel;
            EditorGUILayout.EndVertical();
        }
    }
}

[CustomPropertyDrawer(typeof(Wave))]
public class WaveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        SerializedProperty m_SpawnObjects = property.FindPropertyRelative("spawnObjects");
        
        if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 2;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(property.FindPropertyRelative("duration"), new GUIContent("Duration"));
            // EditorGUILayout.PropertyField(property.FindPropertyRelative("spawnObjects"), new GUIContent("Spawn Objects"));
            
            EditorGUILayout.PropertyField(m_SpawnObjects.FindPropertyRelative("Array.size"), new GUIContent("Spawn Object Count"));
            for (int j = 0; j < m_SpawnObjects.arraySize; ++j)
            {
                GUI.color = Color.gray;
                GUILayout.BeginVertical("", new GUIStyle(GUI.skin.box));
                GUI.color = Color.white;
                EditorGUILayout.PropertyField(m_SpawnObjects.GetArrayElementAtIndex(j), new GUIContent("Spawn Object " + j));
                GUILayout.EndVertical();
            }
            EditorGUI.indentLevel = indent;
            EditorGUILayout.EndVertical();
        }
    }
}