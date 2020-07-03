﻿using BWolf.Examples.SquadFormations.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace BWolf.Examples.SquadFormations.Selection
{
    public class SelectableObject : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float hoverDecalSize = 1.1f;

        [SerializeField]
        private float selectDecalSize = 0.9f;

        [SerializeField]
        private Color decalColor = Color.white;

        [Header("Events")]
        [SerializeField]
        private OnInteraction OnInteract = null;

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

        public Color DecalColor
        {
            get { return decalColor; }
        }

        private ISelectionCallbacks selectionDecal = null;

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

        public void AssignSelectionDecal(ISelectionCallbacks decalInterface)
        {
            selectionDecal = decalInterface;
        }

        public void RetractSelectionDecal()
        {
            selectionDecal = null;
        }

        /// <summary>Set the selectable object to selected</summary>
        public void Select()
        {
            IsSelected = true;
            selectionDecal.OnSelect();
        }

        /// <summary>Set the selectable object to deselected</summary>
        public void Deselect()
        {
            IsSelected = false;
            selectionDecal.OnDeselect();
        }

        /// <summary>Set the selectable object to hovered</summary>
        public void HoverStart()
        {
            IsHovered = true;
            selectionDecal.OnHoverStart();
        }

        /// <summary>Set the selectable object to not hovered</summary>
        public void HoverEnd()
        {
            IsHovered = false;
            selectionDecal.OnHoverEnd();
        }

        public void Interact(Interaction interaction)
        {
            OnInteract.Invoke(interaction);
        }

        [System.Serializable]
        private class OnInteraction : UnityEvent<Interaction> { }
    }
}