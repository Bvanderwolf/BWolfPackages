using UnityEditor;
using UnityEngine;

namespace BWolf.PlayerStatistics.Editor
{
    /// <summary>
    /// Draws a custom inspector for the player stats property ensuring only the
    /// backing list is displayed and no unnecessary foldouts.
    /// </summary>
    [CustomPropertyDrawer(typeof(PlayerStats))]
    public class PlayerStatsDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_list"), label, true);
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty list = property.FindPropertyRelative("_list");
            
            EditorGUI.BeginChangeCheck();
            property.serializedObject.Update();
            
            EditorGUI.PropertyField(position, list, label, true);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
        }
    }
}
