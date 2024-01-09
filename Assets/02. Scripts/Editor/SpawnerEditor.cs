using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner)), CanEditMultipleObjects]
public class SpawnerEditor : Editor
{
    private SerializedProperty m_SpawnObjects;
    private SerializedProperty m_prefab;

    private void OnEnable()
    {
        m_SpawnObjects = serializedObject.FindProperty("spawnObjects");
        m_prefab = serializedObject.FindProperty("prefab");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Spawner myTarget = (Spawner)target;

        myTarget.isWave = EditorGUILayout.Toggle("Is Wave", myTarget.isWave);
        m_SpawnObjects = myTarget.isWave ? serializedObject.FindProperty("waves") : m_SpawnObjects = serializedObject.FindProperty("spawnObjects");
        
        EditorGUILayout.PropertyField(m_SpawnObjects.FindPropertyRelative("Array.size"), new GUIContent("List Count"));
        EditorGUI.indentLevel = 1;
        for (int i = 0; i < m_SpawnObjects.arraySize; ++i)
        {
            EditorGUILayout.PropertyField(m_SpawnObjects.GetArrayElementAtIndex(i), new GUIContent(i.ToString()));
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

        if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
        {
            // if (position.height > 16f)
            // {
            //     position.height = 16f;
            //     EditorGUI.indentLevel += 1;
            //     Rect contentPosition = EditorGUI.IndentedRect(position);
            //     contentPosition.y = 18f;
            // }

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 2;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(property.FindPropertyRelative("prefab"), new GUIContent("Prefab"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("type"), new GUIContent("Type"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("percent"), new GUIContent("Percent"));
            EditorGUI.indentLevel = indent;
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

        if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 2;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(property.FindPropertyRelative("duration"), new GUIContent("Duration"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("spawnObjects"), new GUIContent("Spawn Objects"));
            EditorGUI.indentLevel = indent;
            EditorGUILayout.EndVertical();
        }
    }
}