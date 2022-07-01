// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Utilities.UI
{
    /// <summary>Utility class for managing a draggable number slider</summary>
    public class NumberSlider : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [Header("Settings")]
        [SerializeField, Tooltip("time for slider to snap back to closed number on release")]
        private float easeTime = 0.125f;

        [SerializeField, Tooltip("max positive/negative number represented by the slider. e.g. 9 means -9 to 9")]
        private int maxNumber = 9;

        [Header("References")]
        [SerializeField]
        private GameObject prefabNumber = null;

        private RectTransform rectTransform;

        private float offsetX;
        private float startX;

        private float minAnchorX;
        private float maxAnchorX;

        private bool isEasing;

        private Transform[] numberTransforms;
        private Vector3[] configurationPositions;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            startX = rectTransform.position.x;

            RectTransform numberTransform = (RectTransform)prefabNumber.transform;
            numberTransform.sizeDelta = ((RectTransform)transform.parent).sizeDelta;

            float numberHeight = numberTransform.rect.height;
            int childCount = maxNumber * 2 + 1;

            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = childCount * numberHeight;
            rectTransform.sizeDelta = sizeDelta;

            float slideWidth = (rectTransform.rect.width - numberHeight) * 0.5f;
            minAnchorX = -slideWidth;
            maxAnchorX = slideWidth;

            int displayNumber = -maxNumber;
            numberTransforms = new Transform[childCount];
            configurationPositions = new Vector3[numberTransforms.Length];
            for (int i = 0; i < numberTransforms.Length; i++)
            {
                Transform number = Instantiate(prefabNumber, transform).transform;
                number.GetComponent<Text>().text = displayNumber++.ToString();
                numberTransforms[i] = number;
                configurationPositions[i] = new Vector3(minAnchorX + (numberHeight * i), rectTransform.anchoredPosition.y);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isEasing)
            {
                Vector3 position = rectTransform.position;
                position.x = eventData.position.x - offsetX;
                rectTransform.position = position;

                Vector3 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, minAnchorX, maxAnchorX);
                rectTransform.anchoredPosition = anchoredPosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isEasing)
            {
                Vector3 closestPosition = GetClosestConfigurationPosition();
                StartCoroutine(EaseTowardsPosition(closestPosition));
            }
        }

        /// <summary>Returns the current value displayed by the slider as a double</summary>
        public double GetValue()
        {
            EventSystem eventSystem = EventSystem.current;
            PointerEventData data = new PointerEventData(eventSystem);
            data.position = transform.parent.position;

            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(data, results);

            return double.Parse(results[0].gameObject.GetComponent<Text>().text);
        }

        /// <summary>Returns an enumerator that eases the slider towards given position</summary>
        private IEnumerator EaseTowardsPosition(Vector3 position)
        {
            isEasing = true;

            LerpOf<Vector3> ease = new LerpOf<Vector3>(rectTransform.anchoredPosition, position, easeTime);
            while (ease.Continue())
            {
                rectTransform.anchoredPosition = Vector3.Lerp(ease.initial, ease.target, ease.Percentage);
                yield return null;
            }

            //save x offset so the user can start sliding from this position
            offsetX = startX - rectTransform.position.x;

            isEasing = false;
        }

        /// <summary>Returns the closest stored configuration position to current position</summary>
        private Vector3 GetClosestConfigurationPosition()
        {
            Vector3 point = rectTransform.anchoredPosition;
            Vector3 closestPosition = Vector3.zero;
            float closestSqrDistance = Mathf.Infinity;

            for (int i = 0; i < configurationPositions.Length; i++)
            {
                Vector3 position = configurationPositions[i];
                float sqrDistance = (point - position).sqrMagnitude;
                if (sqrDistance < closestSqrDistance)
                {
                    closestPosition = position;
                    closestSqrDistance = sqrDistance;
                }
            }

            return closestPosition;
        }
    }
}