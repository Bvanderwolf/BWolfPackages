using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class PlayerColorPicker : MonoBehaviour
    {
        [Header("Color Palette")]
        [SerializeField]
        private Color[] palette = new Color[colorButtonCount];

        [Header("Selectability Settings")]
        [SerializeField]
        private float normalAlpha = 0.15f;

        [SerializeField]
        private float highlightAlpha = 0.75f;

        [SerializeField]
        private float pressedAlpha = 1f;

        [SerializeField]
        private float selectedAlpha = 0.15f;

        [SerializeField]
        private float disabledAlpha = 0f;

        private const int colorButtonCount = 9;

        private Selectable[] selectableColors = new Selectable[colorButtonCount];

        private Button colorButtonToModify = null;

        private void Awake()
        {
            gameObject.SetActive(false);

            for (int i = 0; i < selectableColors.Length; i++)
            {
                selectableColors[i] = transform.GetChild(i).GetComponent<Selectable>();
                selectableColors[i].colors = CreateColorBlock(palette[i]);
            }
        }

        private void OnDisable()
        {
            colorButtonToModify = null;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool picked = false;
                GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
                for (int i = 0; i < selectableColors.Length; i++)
                {
                    if (selectedGameObject == selectableColors[i].gameObject)
                    {
                        OnColorPicked(selectableColors[i].colors);
                        picked = true;
                        break;
                    }
                }

                if (!picked)
                {
                    Cancel();
                }
            }
        }

        private void OnColorPicked(ColorBlock colors)
        {
            colorButtonToModify.colors = colors;
            Cancel();
        }

        private void Cancel()
        {
            colorButtonToModify.interactable = true;
            gameObject.SetActive(false);
        }

        private ColorBlock CreateColorBlock(Color c)
        {
            return new ColorBlock
            {
                normalColor = new Color(c.r, c.g, c.b, normalAlpha),
                highlightedColor = new Color(c.r, c.g, c.b, highlightAlpha),
                pressedColor = new Color(c.r, c.g, c.b, pressedAlpha),
                selectedColor = new Color(c.r, c.g, c.b, selectedAlpha),
                disabledColor = new Color(c.r, c.g, c.b, disabledAlpha),
                colorMultiplier = 1,
                fadeDuration = 0.1f
            };
        }

        public void SetColorButtonToModify(Button colorButton)
        {
            colorButtonToModify = colorButton;
            colorButtonToModify.interactable = false;
        }
    }
}