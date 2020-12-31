using BWolf.Utilities.PlayerProgression.PlayerProps;
using UnityEditor;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression
{
    [CustomEditor(typeof(PlayerPropertiesAsset))]
    public class PlayerPropertiesEditor : Editor
    {
        private const string BUTTON_TEXT = "Restore all player properties";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(25f);
            GUIStyle style = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fontStyle = FontStyle.Italic, padding = new RectOffset(15, 15, 5, 5) };

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(BUTTON_TEXT, style, GUILayout.ExpandWidth(false)))
            {
                PlayerPropertiesAsset asset = target as PlayerPropertiesAsset;
                asset.Restore();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}