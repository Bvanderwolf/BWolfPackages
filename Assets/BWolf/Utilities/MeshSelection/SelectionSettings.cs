using UnityEngine;

namespace BWolf.MeshSelecting
{
    /// <summary>
    /// The settings used for mesh selection. Create an asset of this in the root of a folder
    /// named Resources if you are not planning on adding the <see cref="MeshSelector"/>
    /// to a custom scene. 
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SelectionSettings), menuName = "MeshSelection/Settings")]
    public class SelectionSettings : ScriptableObject
    {
        /// <summary>
        /// The key that, when pressed, determines whether to reset the
        /// current selection before selecting new game objects.
        /// </summary>
        public KeyCode inclusiveSelectKey = KeyCode.LeftShift;
        
        /// <summary>
        /// Determines how many pixels the initial mouse click needs
        /// to be from the dragged position to create a selection box.
        /// </summary>
        public float dragThreshold = 40f;

        /// <summary>
        /// The color of the selection box.
        /// </summary>
        public Color boxColor = new Color(0.8f, 0.8f, 0.95f, 0.25f);
        
        /// <summary>
        /// The color of the border of the selection box.
        /// </summary>
        public Color boxBorderColor = new Color(0.8f, 0.8f, 0.95f);

        /// <summary>
        /// The thickness of the border of the selection box in pixels.
        /// </summary>
        public float boxBorderThickness = 2f;
    }
}
