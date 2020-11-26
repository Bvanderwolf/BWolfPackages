// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Utilities.UI
{
    /// <summary>A utility script to be added to a draggable image</summary>
    [RequireComponent(typeof(Image))]
    public class DraggableImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Settings")]
        [SerializeField]
        private float speed = 50.0f;

        [SerializeField]
        private AxisConstrained constrained = AxisConstrained.None;

        private bool isDragging;

        public bool IsDraggable { get; private set; } = true;

        [Header("Events")]
        public DraggableEvent OnDragBegin;

        public DraggableEvent OnDragEnd;
        public DraggableEvent OnHoverBegin;
        public DraggableEvent OnHoverEnd;

        /// <summary>Sets whether this image is draggable or not</summary>
        public void SetDraggability(bool value)
        {
            IsDraggable = value;
            if (isDragging && !value)
            {
                OnDragEnd?.Invoke(new PointerEventData(EventSystem.current));
            }
        }

        /// <summary>Sets given axis constrained to this draggable image</summary>
        public void SetConstrained(AxisConstrained constrained)
        {
            this.constrained = constrained;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsDraggable)
            {
                isDragging = true;
                OnDragBegin?.Invoke(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsDraggable)
            {
                Vector3 point = eventData.position;
                ConstrainPosition(ref point);
                transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime * speed);
            }
        }

        /// <summary>Constrains the original position based on current axis constrained</summary>
        private void ConstrainPosition(ref Vector3 original)
        {
            switch (constrained)
            {
                case AxisConstrained.X:
                    original.x = transform.position.x;
                    break;

                case AxisConstrained.Y:
                    original.y = transform.position.y;
                    break;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsDraggable)
            {
                isDragging = false;
                OnDragEnd?.Invoke(eventData);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverBegin?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverEnd?.Invoke(eventData);
        }

        [System.Serializable]
        public class DraggableEvent : UnityEvent<PointerEventData>
        {
        }
    }

    public enum AxisConstrained
    {
        None,
        X,
        Y
    }
}