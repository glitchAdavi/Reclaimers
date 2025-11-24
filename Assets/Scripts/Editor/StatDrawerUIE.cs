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

        float size = (position.width - 10) / 3;

        var baseVal = new Rect(position.x, position.y, size, position.height);
        var addVal = new Rect(position.x + (position.width / 3) + 3, position.y, size, position.height);
        var multVal = new Rect(position.x + ((position.width / 3) + 3) * 2, position.y, size, position.height);

        EditorGUI.PropertyField(baseVal, property.FindPropertyRelative("baseVal"), GUIContent.none);
        EditorGUI.PropertyField(addVal, property.FindPropertyRelative("addVal"), GUIContent.none);
        EditorGUI.PropertyField(multVal, property.FindPropertyRelative("multVal"), GUIContent.none);


        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
