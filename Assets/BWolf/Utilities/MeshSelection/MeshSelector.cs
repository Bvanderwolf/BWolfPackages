using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    /// <summary>
    /// A mono behaviour to be dispatched by the static <see cref="MeshSelection"/> class or
    /// to be added to a scene to manage selection of 3D game objects in the scene.
    /// </summary>
    public sealed class MeshSelector : MonoBehaviour
    {
        /// <summary>
        /// Fired when an object is clicked.
        /// </summary>
        public event Action<GameObject> Clicked;

        /// <summary>
        /// Fired when the selection has changed after
        /// a box selection has ended.
        /// </summary>
        public event Action SelectionChanged;

        /// <summary>
        /// The selection condition determining if a found collider is fit for selection.
        /// Will select everything if not set.
        /// </summary>
        public Func<Collider, bool> SelectionCondition
        {
            set => _meshCaster.condition = value;
        }

        /// <summary>
        /// The camera used to cast from.
        /// </summary>
        public Camera SelectionCamera
        {
            set => _meshCaster.camera = value;
        }
        
        /// <summary>
        /// Whether a drag selection is currently in progress.
        /// </summary>
        public bool IsDragSelecting => _isDragSelecting;
        
        /// <summary>
        /// The currently selected game objects.
        /// </summary>
        public GameObject[] Selection => _currentSelection.ToArray();
        
        /// <summary>
        /// The amount of selected objects.
        /// </summary>
        public int SelectionCount => _currentSelection.Count;

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
        private readonly List<GameObject> _currentSelection = new List<GameObject>();

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
        private SelectionMeshCaster _meshCaster;

        /// <summary>
        /// Initializes the box caster instance.
        /// </summary>
        private void Awake()
        {
            _meshCaster = new SelectionMeshCaster(MeshSelection.SelectionCamera);
            _meshCaster.Selected += OnSelectionChanged;

            enabled = false;
        }

        /// <summary>
        /// Clears the selection and resets state.
        /// </summary>
        private void OnDisable()
        {
            ClearSelection();

            _isDragSelecting = false;
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
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            int previousCount = _currentSelection.Count;
            
            if (!Physics.Raycast(ray, out RaycastHit hitInfo) || !IsSelectableCollider(hitInfo.collider))
            {
                ClearSelection();
                if (previousCount != _currentSelection.Count)
                    SelectionChanged?.Invoke();
                return;
            }

            GameObject clickedGameObject = hitInfo.transform.gameObject;
            GameObject previousGameObject = _currentSelection.FirstOrDefault();

            // Fire event for subscribers.
            Clicked?.Invoke(clickedGameObject);
            
            // Invoke implementation method if hit object implements interface.
            ISelectableMesh selectableMesh = clickedGameObject.GetComponent<ISelectableMesh>();
            selectableMesh?.OnClick();
            
            // Clear the selection outside of the clicked game object if the inclusive key wasn't pressed.
            if (!Input.GetKey(_inclusiveSelectKey))
                ClearSelection(clickedGameObject);

            // If the selection does not already contain the clicked game object, add it and invoke interface implementation.
            if (!_currentSelection.Contains(clickedGameObject))
            {
                selectableMesh?.OnSelect();
                _currentSelection.Add(clickedGameObject);
            }

            // If the selection has changed after all this, invoke the corresponding event.
            if (previousCount != _currentSelection.Count || previousGameObject != clickedGameObject)
                SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Called when colliders have been selected by the selection box cast
        /// to invoke implementations of the ISelectableMesh interface 
        /// </summary>
        /// <param name="newSelection">The new selection of colliders.</param>
        private void OnSelectionChanged(Collider[] newSelection)
        {
            GameObject[] newlySelectedGameObjects = newSelection.Select(selected => selected.gameObject).ToArray();
            int selectionCount = _currentSelection.Count;
            
            // Clear the selection outside of the newly selected game objects if the inclusive key wasn't pressed.
            if (!Input.GetKey(_inclusiveSelectKey))
                ClearSelection(newlySelectedGameObjects);

            // For each newly selected game object, check if it isn't already selected. If not, add it 
            // and invoke its interface implementation.
            for (int i = 0; i < newlySelectedGameObjects.Length; i++)
            {
                GameObject newlySelectedGameObject = newlySelectedGameObjects[i];
                if (_currentSelection.Contains(newlySelectedGameObject))
                    continue;
                
                newlySelectedGameObject.GetComponent<ISelectableMesh>()?.OnSelect();
                _currentSelection.Add(newlySelectedGameObject);
            }
            
            // If the selection has changed after all this, invoke the corresponding event.
            if (selectionCount != _currentSelection.Count)
                SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Clears the current selection selection of game objects, skipping game objects
        /// given as exclusive.
        /// </summary>
        /// <param name="excludedGameObjects">Game objects to exclude.</param>
        private void ClearSelection(params GameObject[] excludedGameObjects)
        {
            for (int i = _currentSelection.Count - 1; i >= 0; i--)
            {
                // Skip destroyed or exclusive game objects from being removed.
                GameObject selectedGameObject = _currentSelection[i];
                if (selectedGameObject == null || excludedGameObjects.Any(go => go == selectedGameObject))
                    continue;

                // Removed game objects have their ISelectableMesh implementation method invoked.
                selectedGameObject.GetComponent<ISelectableMesh>()?.OnDeselect();
                _currentSelection.RemoveAt(i);
            }
        }

        /// <summary>
        /// Called when the user has ended a drag selection to do a box cast
        /// to select game objects in the scene.
        /// </summary>
        /// <param name="mousePosition">The mouse position ended on.</param>
        private void OnDragSelectEnd(Vector2 mousePosition)
        {
            // Retrieve the corners of the selection box on screen to start a mesh cast.
            Vector2[] corners = SelectionUtility.CornersFromPositions(_initialMousePosition, mousePosition);
            
            _meshCaster.Cast(corners);
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

        private bool IsSelectableCollider(Collider colliderToCheck) =>
            _meshCaster.condition == null || _meshCaster.condition.Invoke(colliderToCheck);

        
        public static MeshSelector CreateFromSettings(SelectionSettings settings)
        {
            GameObject gameObject = new GameObject("~MeshSelector");
            MeshSelector selector = gameObject.AddComponent<MeshSelector>();
            selector._inclusiveSelectKey = settings.inclusiveSelectKey;
            selector._dragThreshold = settings.dragThreshold;
            selector._boxColor = settings.boxColor;
            selector._boxBorderColor = settings.boxBorderColor;
            selector._boxBorderThickness = settings.boxBorderThickness;
            
            DontDestroyOnLoad(gameObject);

            return selector;
        }
    }
}
