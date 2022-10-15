using System.Text;
using UnityEditor;
using UnityEngine;

namespace BWolf.SceneSearch.Editor
{
    public static class ScenePathMenuExtensions
    {
        [MenuItem("GameObject/Copy Path", true)]
        private static bool IsSelectingRootGameObject() => Selection.activeTransform.parent == null;

        [MenuItem("GameObject/Copy Path", false, 0)]
        private static void CopyGameObjectHierarchyPath(MenuCommand command)
        {
            GameObject gameObject = (GameObject)command.context;
            Transform transform = gameObject.transform;
            StringBuilder builder = new StringBuilder(transform.name);
            
            while (transform.parent != null)
            {
                transform = transform.parent;
                InsertNameToPath(transform.name);
            }
            
            InsertNameToPath(gameObject.scene.name);
            GUIUtility.systemCopyBuffer = builder.ToString();
            
            void InsertNameToPath(string name)
            {
                builder.Insert(0, '/');
                builder.Insert(0, name);
            }
        }
    }
}
