using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Stat))]
public class StatDrawerUIE : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);


        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float size = (position.width - 36) / 6;

        var baseVal = new Rect(position.x, position.y, size, position.height);
        var multVal = new Rect(position.x + (position.width / 6) + 6, position.y, size, position.height);
        var addTotalVal = new Rect(position.x + ((position.width / 6) + 6) * 2, position.y, size, position.height);
        var multTotalVal = new Rect(position.x + ((position.width / 6) + 6) * 3, position.y, size, position.height);
        var minVal = new Rect(position.x + ((position.width / 6) + 6) * 4, position.y, size, position.height);
        var maxVal = new Rect(position.x + ((position.width / 6) + 6) * 5, position.y, size, position.height);

        EditorGUI.PropertyField(baseVal, property.FindPropertyRelative("_base"), GUIContent.none);
        EditorGUI.PropertyField(multVal, property.FindPropertyRelative("_mult"), GUIContent.none);
        EditorGUI.PropertyField(addTotalVal, property.FindPropertyRelative("_addTotal"), GUIContent.none);
        EditorGUI.PropertyField(multTotalVal, property.FindPropertyRelative("_multTotal"), GUIContent.none);
        EditorGUI.PropertyField(minVal, property.FindPropertyRelative("_min"), GUIContent.none);
        EditorGUI.PropertyField(maxVal, property.FindPropertyRelative("_max"), GUIContent.none);


        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
