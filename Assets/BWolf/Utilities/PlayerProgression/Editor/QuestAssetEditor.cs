using BWolf.Utilities.PlayerProgression.Quests;
using UnityEditor;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression
{
    [CustomEditor(typeof(QuestAsset))]
    public class QuestAssetEditor : Editor
    {
        private const string BUTTON_TEXT = "Restore all quests";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(25f);
            GUIStyle style = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fontStyle = FontStyle.Italic, padding = new RectOffset(15, 15, 5, 5) };

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(BUTTON_TEXT, style, GUILayout.ExpandWidth(false)))
            {
                QuestAsset asset = target as QuestAsset;
                asset.Restore();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}