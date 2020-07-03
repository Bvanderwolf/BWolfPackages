using UnityEngine;

namespace BWolf.Examples.SquadFormations.Selection
{
    public class SelectionDecal : MonoBehaviour, ISelectionCallbacks
    {
        [Header("References")]
        [SerializeField]
        private Projector projectorHoverDecal = null;

        [SerializeField]
        private Projector projectorSelectDecal = null;

        public SelectableObject AssignedSelectable { get; private set; }

        private void Update()
        {
            if (AssignedSelectable != null)
            {
                transform.position = AssignedSelectable.transform.position;
            }
        }

        /// <summary>Set up the decal with the given selectable object and color</summary>
        public void Setup(SelectableObject selectableObject)
        {
            AssignedSelectable = selectableObject;
            AssignedSelectable.AssignSelectionDecal(this);

            projectorHoverDecal.orthographicSize = AssignedSelectable.HoverDecalSize;
            projectorSelectDecal.orthographicSize = AssignedSelectable.SelectDecalSize;

            projectorHoverDecal.material.color = AssignedSelectable.DecalColor;
            projectorSelectDecal.material.color = AssignedSelectable.DecalColor;

            projectorHoverDecal.gameObject.SetActive(false);
            projectorSelectDecal.gameObject.SetActive(false);

            gameObject.SetActive(true);
        }

        public void Retract()
        {
            AssignedSelectable.RetractSelectionDecal();
            AssignedSelectable = null;
            gameObject.SetActive(false);
        }

        public void OnSelect()
        {
            projectorSelectDecal.gameObject.SetActive(true);
        }

        public void OnDeselect()
        {
            projectorSelectDecal.gameObject.SetActive(false);
        }

        public void OnHoverStart()
        {
            projectorHoverDecal.gameObject.SetActive(true);
        }

        public void OnHoverEnd()
        {
            projectorHoverDecal.gameObject.SetActive(false);
        }
    }
}