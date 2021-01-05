// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

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
                AudioProfileSO profile = target as AudioProfileSO;
                if (profile.ReAssertGroupVolumes())
                {
                    profile.SaveGroupVolumesToFile();
                }
            }
        }
    }
}