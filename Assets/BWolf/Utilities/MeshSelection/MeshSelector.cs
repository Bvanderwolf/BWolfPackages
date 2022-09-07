﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    public class MeshSelector : MonoBehaviour
    {
        public event Action<RaycastHit> Clicked;

        public event Action SelectionChanged;

        private readonly KeyCode _inclusiveSelectKey = KeyCode.LeftShift;
        
        private readonly HashSet<GameObject> _selection = new HashSet<GameObject>();

        private Vector2 _firstMousePosition;

        private float _selectionDepth = 1f;

        private int _maxSelectCount = 20;
        
        private float _dragThreshold = 40f;

        private bool _isDragSelecting;

        private RaycastHit[] _selectionHits;

        private void Awake()
        {
            _selectionHits = new RaycastHit[_maxSelectCount];
        }

        private void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            
            // If the user pressed the left mouse button, set the first mouse position.
            if (Input.GetMouseButtonDown(0))
                _firstMousePosition = mousePosition;

            // If the user holds the left mouse button and crosses the drag threshold, set the drag selecting flag.
            if (Input.GetMouseButton(0) && CrossesDragThreshold(mousePosition))
                _isDragSelecting = true;

            // If the user is not releasing the mouse button, do nothing.
            if (!Input.GetMouseButtonUp(0))
                return;

            if (_isDragSelecting)
                OnDragSelectEnd(mousePosition);
            else
                OnClick(mousePosition);
        }

        private void OnGUI()
        {
            if (!_isDragSelecting)
                return;
            
            Rect dragRect = SelectionUtility.ScreenRectFromPositions(_firstMousePosition, Input.mousePosition);
            SelectionUtility.DrawRect(dragRect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            SelectionUtility.DrawBorder(dragRect, 2, new Color(0.8f, 0.8f, 0.95f));
        }

        private void OnClick(Vector3 mousePosition)
        {
            Ray ray = (MeshSelection.camera ?? Camera.main).ScreenPointToRay(mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                _selection.Clear();
                return;
            }
            
            // Fire events for subscribers.
            Clicked?.Invoke(hitInfo);
            
            // Invoke implementation method if hit object implements interface.
            ISelectableMesh selectableMesh = hitInfo.transform.GetComponent<ISelectableMesh>();
            selectableMesh?.OnClick();
            
            // Clear selection if the inclusive select key is not pressed.
            if (!Input.GetKey(_inclusiveSelectKey))
                _selection.Clear();
            
            _selection.Add(hitInfo.transform.gameObject);
        }

        private void OnDragSelectEnd(Vector2 mousePosition)
        {
            Camera selectionCamera = MeshSelection.camera ?? Camera.main;
            Vector2[] corners = SelectionUtility.CornersFromPositions(_firstMousePosition, mousePosition);
            Ray[] rays = SelectionUtility.RaysFromCorners(corners, selectionCamera);
            BoxCastInput input = BoxCastInput.FromRays(rays, _selectionDepth);

            // Clear selection if the inclusive select key is not pressed.
            if (!Input.GetKey(_inclusiveSelectKey))
                _selection.Clear();
            
            int hitCount = Physics.BoxCastNonAlloc(input.center, input.halfExtends, input.direction, _selectionHits);
            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = _selectionHits[i];
                ISelectableMesh selectableMesh = hit.transform.GetComponent<ISelectableMesh>();
                selectableMesh?.OnSelect();
                _selection.Add(hit.transform.gameObject);
            }

            _isDragSelecting = false;
        }

        private bool CrossesDragThreshold(Vector3 mousePosition)
            => Vector2.Distance(_firstMousePosition, mousePosition) > _dragThreshold;
    }
}
