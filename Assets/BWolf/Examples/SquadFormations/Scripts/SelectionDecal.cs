using UnityEngine;

namespace BWolf.Examples.SquadFormations
{
    public class SelectionDecal : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Projector projectorHoverDecal = null;

        [SerializeField]
        private Projector projectorSelectDecal = null;

        public SelectableObject SelectedSelectable { get; private set; }

        /// <summary>Set up the decal with the given selectable object and color</summary>
        public void Setup(SelectableObject selectableObject, Color color)
        {
            this.SelectedSelectable = selectableObject;

            projectorHoverDecal.orthographicSize = this.SelectedSelectable.HoverDecalSize;
            projectorSelectDecal.orthographicSize = this.SelectedSelectable.SelectDecalSize;

            projectorHoverDecal.material.color = color;
            projectorSelectDecal.material.color = color;
        }
    }
}