using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    /// <summary>
    /// A mono behaviour to be dispatched by the static <see cref="MeshSelection"/> class or
    /// to be added to a scene to manage selection of 3D game objects in the scene.
    /// </summary>
    public class MeshSelector : MonoBehaviour
    {
        /// <summary>
        /// Fired when an object is clicked.
        /// </summary>
        public event Action<GameObject> Clicked;

        /// <summary>
        /// Fired when an object is selected as part of
        /// a box selection.
        /// </summary>
        public event Action<GameObject> Selected;

        /// <summary>
        /// The key that, when pressed, determines whether to reset the
        /// current selection before selecting new game objects.
        /// </summary>
        [SerializeField]
        private KeyCode _inclusiveSelectKey = KeyCode.LeftShift;
        
        /// <summary>
        /// Determines how many pixels the initial mouse click needs
        /// to be from the dragged position to create a selection box.
        /// </summary>
        [SerializeField]
        private float _dragThreshold = 40f;

        /// <summary>
        /// The color of the selection box.
        /// </summary>
        [SerializeField]
        private Color _boxColor = new Color(0.8f, 0.8f, 0.95f, 0.25f);
        
        /// <summary>
        /// The color of the border of the selection box.
        /// </summary>
        [SerializeField]
        private Color _boxBorderColor = new Color(0.8f, 0.8f, 0.95f);

        /// <summary>
        /// The thickness of the border of the selection box in pixels.
        /// </summary>
        [SerializeField]
        private float _boxBorderThickness = 2f;
        
        /// <summary>
        /// The current selection of game objects.
        /// </summary>
        private readonly HashSet<GameObject> _selection = new HashSet<GameObject>();

        /// <summary>
        /// The initial mouse position when left clicked on screen by the user.
        /// </summary>
        private Vector2 _initialMousePosition;

        /// <summary>
        /// Whether the user is drag selecting.
        /// </summary>
        private bool _isDragSelecting;

        /// <summary>
        /// The box caster instance helping with generating the selection mesh.
        /// </summary>
        private SelectionBoxCaster _boxCaster;

        /// <summary>
        /// Initializes the box caster instance.
        /// </summary>
        private void Awake()
        {
            _boxCaster = new SelectionBoxCaster(MeshSelection.camera ?? Camera.main);
            _boxCaster.Selected += OnSelectedGameObject;
        }

        /// <summary>
        /// Validates mouse input, triggering events when necessary.
        /// </summary>
        private void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            
            // If the user pressed the left mouse button, set the first mouse position.
            if (Input.GetMouseButtonDown(0))
                _initialMousePosition = mousePosition;

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

        /// <summary>
        /// Draws the selection box if the user is drag selecting.
        /// </summary>
        private void OnGUI()
        {
            if (!_isDragSelecting)
                return;
            
            Rect dragRect = SelectionUtility.ScreenRectFromPositions(_initialMousePosition, Input.mousePosition);
            SelectionUtility.DrawRect(dragRect, _boxColor);
            SelectionUtility.DrawBorder(dragRect, _boxBorderThickness, _boxBorderColor);
        }

        /// <summary>
        /// Called when the user has clicked the screen to select a game object.
        /// </summary>
        /// <param name="mousePosition">The mouse position on screen.</param>
        private void OnClick(Vector3 mousePosition)
        {
            Ray ray = (MeshSelection.camera ?? Camera.main).ScreenPointToRay(mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                _selection.Clear();
                return;
            }

            GameObject clickedGameObject = hitInfo.transform.gameObject;
            
            // Fire event for subscribers.
            Clicked?.Invoke(clickedGameObject);
            
            // Invoke implementation method if hit object implements interface.
            ISelectableMesh selectableMesh = clickedGameObject.GetComponent<ISelectableMesh>();
            selectableMesh?.OnClick();
            
            // Clear selection if the inclusive select key is not pressed.
            if (!Input.GetKey(_inclusiveSelectKey))
                _selection.Clear();
            
            _selection.Add(clickedGameObject);
        }

        /// <summary>
        /// Called when a game object has been selected by the selection box cast
        /// to invoke implementations of the ISelectableMesh interface 
        /// </summary>
        /// <param name="selectedGameObject"></param>
        private void OnSelectedGameObject(GameObject selectedGameObject)
        {
            // Fire event for subscribers.
            Selected?.Invoke(selectedGameObject);
            
            // Invoke implementation method if hit object implements interface.
            ISelectableMesh selectableMesh = selectedGameObject.GetComponent<ISelectableMesh>();
            selectableMesh?.OnSelect();

            _selection.Add(selectedGameObject);
        }

        /// <summary>
        /// Called when the user has ended a drag selection to do a box cast
        /// to select game objects in the scene.
        /// </summary>
        /// <param name="mousePosition">The mouse position ended on.</param>
        private void OnDragSelectEnd(Vector2 mousePosition)
        {
            // Clear selection if the inclusive select key is not pressed.
            if (!Input.GetKey(_inclusiveSelectKey))
                _selection.Clear();
            
            // Retrieve the corners of the selection box on screen to start a box cast.
            Vector2[] corners = SelectionUtility.CornersFromPositions(_initialMousePosition, mousePosition);
            _boxCaster.Cast(corners);
            _isDragSelecting = false;
        }

        /// <summary>
        /// Returns whether the given mouse position its distance to the initial mouse position
        /// crosses the drag threshold.
        /// </summary>
        /// <param name="mousePosition">The mouse position to check.</param>
        /// <returns>Whether the drag threshold was crossed.</returns>
        private bool CrossesDragThreshold(Vector3 mousePosition)
            => Vector2.Distance(_initialMousePosition, mousePosition) > _dragThreshold;
    }
}
