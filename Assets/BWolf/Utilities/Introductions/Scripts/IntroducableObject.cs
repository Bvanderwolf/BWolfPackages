using UnityEngine;

namespace BWolf.Utilities.Introductions
{
    using RelativePosition = IntroArrow.RelativePosition;

    public class IntroducableObject : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private string intoTagName = string.Empty;

        [SerializeField]
        private RelativePosition relativeArrowPosition = RelativePosition.Bottom;

        [SerializeField]
        private float introArrowSpacing = 0.0f;

        private GameObject activeArrow;

        public string IntroTagName
        {
            get { return intoTagName; }
        }

        public const string COMPONENT_TAG_NAME = "Introducable";

        private void Awake()
        {
            if (tag != COMPONENT_TAG_NAME)
            {
                Debug.LogWarning($"Object {gameObject} is an introducable object but has no {COMPONENT_TAG_NAME} tag attached");
            }
        }

        private void OnDestroy()
        {
            if (activeArrow != null)
            {
                EndIntroduction();
            }
        }

        public void StartIntroduction()
        {
            RectTransform rectTransform = transform as RectTransform;
            if (rectTransform != null)
            {
                activeArrow = IntroductionManager.Instance.GetArrow(transform);
                activeArrow.GetComponent<IntroArrow>().PlaceRelative(rectTransform, relativeArrowPosition, introArrowSpacing);
            }
            else
            {
                activeArrow = IntroductionManager.Instance.GetArrow();
                activeArrow.GetComponent<IntroArrow>().PlaceRelative(transform, relativeArrowPosition, introArrowSpacing);
            }
        }

        public void EndIntroduction()
        {
            Destroy(activeArrow);
            activeArrow = null;
        }
    }
}