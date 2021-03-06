﻿using BWolf.Wrappers.PhotonSDK;
using BWolf.Wrappers.PhotonSDK.Handlers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper.Main
{
    /// <summary>Component for handling the picking of a player color</summary>
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

        private Dictionary<Color, bool> availableColors = new Dictionary<Color, bool>();

        /// <summary>Returns the first found available color in the availablecolors dictionary</summary>
        public Color FirstAvailableColor
        {
            get
            {
                foreach (Color col in palette)
                {
                    if (availableColors[col])
                    {
                        return col;
                    }
                }
                return default;
            }
        }

        private void Awake()
        {
            gameObject.SetActive(false);

            for (int i = 0; i < selectableColors.Length; i++)
            {
                selectableColors[i] = transform.GetChild(i).GetComponent<Selectable>();
                selectableColors[i].colors = CreateColorBlock(palette[i]);
                selectableColors[i].targetGraphic.color = palette[i];

                availableColors.Add(palette[i], true);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //when a mouse butotn has been pressed it we check whether to close having picked a color or not
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
                    //if no color was picked we fire the cancel event
                    OnCancel();
                }

                gameObject.SetActive(false);
            }
        }

        /// <summary>Rereshes the available colors dictionary with player color property information and makes pickable colors interactable based on that information</summary>
        public void UpdateAvailableColors()
        {
            Dictionary<int, Color> clientColors = NetworkingService.GetPropertiesOfClientsInRoom<Color>(ClientHandler.PlayerColorKey);
            List<Color> unAvailableColors = new List<Color>();
            foreach (Color color in availableColors.Keys)
            {
                if (clientColors.ContainsValue(color))
                {
                    unAvailableColors.Add(color);
                }
            }

            availableColors.Clear();
            for (int i = 0; i < palette.Length; i++)
            {
                availableColors.Add(palette[i], !unAvailableColors.Contains(palette[i]));
            }

            for (int i = 0; i < selectableColors.Length; i++)
            {
                selectableColors[i].interactable = availableColors[selectableColors[i].targetGraphic.color];
            }
        }

        /// <summary>Creates a new color block structure given a single color</summary>
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