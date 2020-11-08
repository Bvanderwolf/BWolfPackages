using UnityEngine;

namespace BWolf.Utilities.Introductions
{
    public class FloatableCanvasElement : MonoBehaviour
    {
        [SerializeField, Min(2.0f)]
        private float amplitude = 2.0f;

        [SerializeField]
        private float frequency = 2.0f;

        private RectTransform rectTransform;

        private float time;
        private Vector2 start;
        private Vector2 up;

        private void Awake()
        {
            rectTransform = (RectTransform)transform;
            time = Time.time;
        }

        private void Start()
        {
            ReStart();
        }

        private void Update()
        {
            time += Time.deltaTime;

            float scalar = amplitude * Mathf.Sin(frequency * time);
            rectTransform.anchoredPosition = start + (up * scalar);
        }

        public void ReStart()
        {
            start = rectTransform.anchoredPosition;
            up = rectTransform.up;
        }
    }
}