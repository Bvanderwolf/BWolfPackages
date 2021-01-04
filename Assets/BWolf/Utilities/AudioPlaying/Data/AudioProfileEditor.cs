using UnityEditor;

namespace BWolf.Utilities.AudioPlaying
{
    [CustomEditor(typeof(AudioProfileSO))]
    public class AudioProfileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            DrawDefaultInspector();

            if (EditorGUI.EndChangeCheck())
            {
                ((AudioProfileSO)target).Validate();
            }
        }
    }
}