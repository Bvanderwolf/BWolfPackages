using UnityEngine;

namespace BWolf.MeshSelecting
{
    [CreateAssetMenu(fileName = nameof(SelectionSettings), menuName = "MeshSelection/Settings")]
    public class SelectionSettings : ScriptableObject
    {
        public bool initializeOnLoad = true;
        
        public bool enableOnLoad = true;

        public bool disableOnLoad = false;

        public KeyCode inclusiveSelectKey = KeyCode.LeftShift;
    }
}
