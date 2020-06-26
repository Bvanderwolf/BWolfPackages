﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

        public Action<Color> OnColorPicked;
        public Action OnCancel;

        private const int colorButtonCount = 9;

        private Selectable[] selectableColors = new Selectable[colorButtonCount];

        private List<Color> avaialableColors = new List<Color>();

        private void Awake()
        {
            gameObject.SetActive(false);

            for (int i = 0; i < selectableColors.Length; i++)
            {
                selectableColors[i] = transform.GetChild(i).GetComponent<Selectable>();
                selectableColors[i].colors = CreateColorBlock(palette[i]);

                avaialableColors.Add(palette[i]);
            }
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
                        OnColorPicked(palette[i]);
                        picked = true;
                        break;
                    }
                }

                if (!picked)
                {
                    OnCancel();
                }

                gameObject.SetActive(false);
            }
        }

        public ColorBlock CreateColorBlock(Color c)
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
    }
}