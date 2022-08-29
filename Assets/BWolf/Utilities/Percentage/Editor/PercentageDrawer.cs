using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Perc))]
public class PercentageDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty clamp = property.FindPropertyRelative("clamp01");

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        property.serializedObject.Update();
        
        OnValueGUI(position, label, clamp.boolValue, property.FindPropertyRelative("value"));
        OnClampGUI(position, clamp);

        EditorGUI.EndProperty();
        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();
    }

    private void OnValueGUI(Rect position, GUIContent label, bool clamp01, SerializedProperty value)
    {
        label.tooltip = "Toggle to clamp value between 0 and 1.";
        position.width -= position.height;

        Rect rect = EditorGUI.PrefixLabel(position, label);
        value.floatValue = clamp01 
            ? EditorGUI.Slider(rect, value.floatValue, 0f, 1f) 
            : EditorGUI.FloatField(rect, value.floatValue);
    }

    private void OnClampGUI(Rect position, SerializedProperty clamp)
    {
        position.x += position.width + EditorGUIUtility.standardVerticalSpacing - position.height;
        position.width = position.height;
        clamp.boolValue = EditorGUI.Toggle(position, clamp.boolValue);
    }
}
