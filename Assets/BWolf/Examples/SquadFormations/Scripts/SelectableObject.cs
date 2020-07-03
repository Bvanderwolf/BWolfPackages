using UnityEngine;

namespace BWolf.Examples.SquadFormations
{
    public class SelectableObject : MonoBehaviour
    {
        public bool IsSelectable { get; private set; }
        public bool IsHovered { get; private set; }
        public bool IsSelected { get; private set; }

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
            SelectionHandler.Instance.RemoveSelectableObject(this);
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
        }

        /// <summary>Set the selectable object to hovered</summary>
        public void HoverStart()
        {
            IsHovered = true;
        }

        /// <summary>Set the selectable object to not hovered</summary>
        public void HoverEnd()
        {
            IsHovered = false;
        }
    }
}