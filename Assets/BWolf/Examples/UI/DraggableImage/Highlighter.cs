using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.UI
{
    public class Highlighter : MonoBehaviour
    {
        [SerializeField]
        private Color color = Color.blue;

        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();

            ToggleHighlight(null);
        }

        public void ToggleHighlight(PointerEventData eventData)
        {
            image.enabled = !image.enabled;
        }

        public void OnDragBegin(PointerEventData eventData)
        {
            image.color = color;
        }

        public void OnDragEnd(PointerEventData eventData)
        {
            image.color = Color.white;
        }
    }
}