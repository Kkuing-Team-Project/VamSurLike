using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(ObjectPool))]
public class ObjectPoolEditor : Editor
{
    private ReorderableList poolList;
    private Dictionary<int, int> foundObjectTypes;
    private string errorEnumName;
    private void OnEnable()
    {
        foundObjectTypes = new Dictionary<int, int>();
        poolList = new ReorderableList(serializedObject, serializedObject.FindProperty("pools"), true,
            true, true, true);

        poolList.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Pools");
        };

        poolList.onAddCallback = list =>
        {
            ReorderableList.defaultBehaviours.DoAddButton(list);
        
            int index = list.serializedProperty.arraySize - 1;
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            
            if (element.FindPropertyRelative("type").enumNames.Length > index)
                element.FindPropertyRelative("type").enumValueIndex = index;
            element.FindPropertyRelative("prefab").objectReferenceValue = null;
            element.FindPropertyRelative("size").intValue = 5;
        };
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        serializedObject.Update();
        poolList.drawElementCallback = (rect, index, active, focused) =>
        {
            rect.y += 2;
            var element = poolList.serializedProperty.GetArrayElementAtIndex(index);
            var baseWidth = (rect.width / 3) - 5;
            SerializedProperty typeProperty = element.FindPropertyRelative("type");
            
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, baseWidth, EditorGUIUtility.singleLineHeight),
                typeProperty, GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + baseWidth + 5, rect.y, baseWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("prefab"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + (baseWidth * 2) + 10, rect.y, baseWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("size"), GUIContent.none);

            int enumIndex = typeProperty.enumValueIndex;
            if (!foundObjectTypes.ContainsKey(index))
            {
                if (foundObjectTypes.ContainsValue(enumIndex))
                {
                    errorEnumName = typeProperty.enumNames[enumIndex];
                    EditorGUILayout.HelpBox($"{errorEnumName} is included", MessageType.Error);
                }
                else
                {
                    foundObjectTypes.Add(index, enumIndex);
                }
            }
        };
        foundObjectTypes.Clear();
        
        poolList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        // if (GUI.changed) EditorUtility.SetDirty(myTarget);
    }
}
