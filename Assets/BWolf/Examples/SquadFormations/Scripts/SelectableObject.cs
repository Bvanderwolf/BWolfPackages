using UnityEngine;

namespace BWolf.Examples.SquadFormations
{
    public class SelectableObject : MonoBehaviour
    {
        [SerializeField]
        private float hoverDecalSize = 1.1f;

        [SerializeField]
        private float selectDecalSize = 0.9f;

        public bool IsSelectable { get; private set; }
        public bool IsHovered { get; private set; }
        public bool IsSelected { get; private set; }

        public float HoverDecalSize
        {
            get { return hoverDecalSize; }
        }

        public float SelectDecalSize
        {
            get { return selectDecalSize; }
        }

        private void Awake()
        {
            IsSelectable = true;
        }

        private void Start()
        {
            SelectionHandler.Instance.AddSelectableObject(this);
        }

        private void OnDestroy()
        {
            SelectionHandler.Instance?.RemoveSelectableObject(this);
        }

        /// <summary>Set the selectable object to selected</summary>
        public void Select()
        {
            IsSelected = true;
            print("selected " + name);
        }

        /// <summary>Set the selectable object to deselected</summary>
        public void Deselect()
        {
            IsSelected = false;
            print("deselected " + name);
        }

        /// <summary>Set the selectable object to hovered</summary>
        public void HoverStart()
        {
            IsHovered = true;
            print("hovered " + name);
        }

        /// <summary>Set the selectable object to not hovered</summary>
        public void HoverEnd()
        {
            IsHovered = false;
            print("stopped hovering " + name);
        }
    }
}