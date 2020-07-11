using BWolf.Utilities.SquadFormations.Interactions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BWolf.Utilities.SquadFormations.Selection
{
    /// <summary>Component class for managing selection and hovering of entities in a scene</summary>
    public class SelectionHandler : MonoBehaviour
    {
        public static SelectionHandler Instance { get; private set; }

        [Header("References")]
        [SerializeField]
        private Camera mainCamera = null;

        [SerializeField]
        private EventSystem mainEventSystem = null;

        [Header("Settings")]
        [SerializeField]
        private LayerMask selectableLayers;

        [SerializeField]
        private Color innerColor = Color.clear;

        [SerializeField]
        private Color borderColor = Color.green;

        [SerializeField]
        private float borderThickness = 2f;

        private SelectionDecalHandler decalHandler;
        private InteractionHandler interactionHandler;

        private List<SelectableObject> selectableObjects = new List<SelectableObject>();
        private List<SelectableObject> selectedObjects = new List<SelectableObject>();

        private SelectableObject hoveredObject;

        private Vector2 mousePositionStart;
        private Texture2D selectionBoxTex;
        private bool isSelecting;

        private void Awake()
        {
            Instance = this;
            decalHandler = GetComponent<SelectionDecalHandler>();
            interactionHandler = GetComponent<InteractionHandler>();
        }

        private void Start()
        {
            selectionBoxTex = new Texture2D(1, 1);
            selectionBoxTex.SetPixel(0, 0, Color.white);
            selectionBoxTex.Apply();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            //get whether the mouse was over a ui element and handle selection and hovering based on it
            bool pointerOverUI = mainEventSystem != null && mainEventSystem.IsPointerOverGameObject();
            HandleSelection(pointerOverUI);
            HandleHovering(pointerOverUI);
        }

        private void OnGUI()
        {
            if (isSelecting)
            {
                //draw the selection box when the user is selecting
                DrawSelectionBox(mousePositionStart, Input.mousePosition);
            }
        }

        /// <summary>Starts and ends selection based on mouse input</summary>
        private void HandleSelection(bool pointerOverUI)
        {
            if (pointerOverUI)
            {
                if (isSelecting)
                {
                    //if the user was selecting, finish the selection
                    FinishSelection();
                }
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                //start selecting if the user puts the left mouse button
                mousePositionStart = Input.mousePosition;
                isSelecting = true;
            }

            if (Input.GetMouseButtonUp(0) && isSelecting)
            {
                //finish selection if the left mouse button is released and the user was selecting
                FinishSelection();
            }
        }

        /// <summary>Checks whether the user is hovering over entities in the scene</summary>
        private void HandleHovering(bool pointerOverUI)
        {
            if (pointerOverUI)
            {
                //if pointer was over ui and hovered object isn't null, handle hover end
                if (hoveredObject != null)
                {
                    EndHover();
                }
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, selectableLayers.value))
            {
                SelectableObject selectableObject = hit.collider.GetComponentInParent<SelectableObject>();

                //exit if no selectable object component was found
                if (selectableObject == null) { return; }

                // exit if the selectable object is not in the selectable objects list
                if (selectableObject != null && !selectableObjects.Contains(selectableObject))
                {
                    Debug.LogWarningFormat("selectable object {0} is not in the selectable objects list", selectableObject.name);
                    return;
                }

                // setup hovered object if it wasn't assigned yet
                if (hoveredObject == null)
                {
                    hoveredObject = selectableObject;
                    hoveredObject.HoverStart();
                }
            }
            else
            {
                if (hoveredObject != null)
                {
                    EndHover();
                }
            }
        }

        /// <summary>ends hovering of hovered object</summary>
        private void EndHover()
        {
            hoveredObject.HoverEnd();
            hoveredObject = null;
        }

        /// <summary>Add selectable object to list of selectable objects</summary>
        public void AddSelectableObject(SelectableObject selectableObject)
        {
            if (!selectableObjects.Contains(selectableObject))
            {
                selectableObjects.Add(selectableObject);
                decalHandler.AssignSelectionDecal(selectableObject);
            }
        }

        /// <summary>Remove selectable object from list of selectable objects</summary>
        public void RemoveSelectableObject(SelectableObject selectableObject)
        {
            if (selectableObjects.Contains(selectableObject))
            {
                selectableObjects.Remove(selectableObject);
                decalHandler.RetractSelectionDecal(selectableObject);
            }
        }

        /// <summary>Finishes selection by checking which selectable objects are selected and selecting them</summary>
        private void FinishSelection()
        {
            List<SelectableObject> foundSelectableObjects = new List<SelectableObject>();

            foreach (SelectableObject selectable in selectableObjects)
            {
                if ((IsInSelectionBox(selectable.transform.position) || selectable.IsHovered) && selectable.IsSelectable)
                {
                    foundSelectableObjects.Add(selectable);
                }
            }

            // Select found objects
            SelectObjects(foundSelectableObjects);

            // End selection box
            isSelecting = false;
        }

        /// <summary>Select given selectable objects</summary>
        private void SelectObjects(List<SelectableObject> newSelectedObjects)
        {
            //deselect current selected objects
            foreach (SelectableObject selectable in selectedObjects)
            {
                selectable.Deselect();
            }

            //reset and refresh selected objects with newly selected objects
            selectedObjects.Clear();
            foreach (SelectableObject selectable in newSelectedObjects)
            {
                selectedObjects.Add(selectable);
            }

            //select newly selected objects
            foreach (SelectableObject selectable in selectedObjects)
            {
                selectable.Select();
            }

            interactionHandler.StoreSelectedObjects(selectedObjects);
        }

        /// <summary>Return if given worldPosition is within selection box</summary>
        private bool IsInSelectionBox(Vector3 worldPosition)
        {
            Bounds viewportBounds = GetViewportBounds(mousePositionStart, Input.mousePosition);

            Vector3 position = mainCamera.WorldToViewportPoint(worldPosition);
            bool contains = viewportBounds.Contains(position);
            return contains;
        }

        /// <summary>Return viewport bounds from two given positions</summary>
        private Bounds GetViewportBounds(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            Vector3 v1 = mainCamera.ScreenToViewportPoint(screenPosition1);
            Vector3 v2 = mainCamera.ScreenToViewportPoint(screenPosition2);
            Vector3 min = Vector3.Min(v1, v2);
            Vector3 max = Vector3.Max(v1, v2);
            min.z = mainCamera.nearClipPlane;
            max.z = mainCamera.farClipPlane;

            Bounds bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
        }

        /// <summary>Draw selection box using mouse start position and mouse current position as corners</summary>
        private void DrawSelectionBox(Vector3 start, Vector3 current)
        {
            start.y = Screen.height - start.y;
            current.y = Screen.height - current.y;

            // Calculate corners
            Vector3 topLeft = Vector3.Min(start, current);
            Vector3 bottomRight = Vector3.Max(start, current);

            // Create rect
            Rect rect = Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);

            // Draw
            GUI.color = innerColor;
            GUI.DrawTexture(rect, selectionBoxTex); // Center
            GUI.color = borderColor;
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, borderThickness), selectionBoxTex); // Top
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, borderThickness, rect.height), selectionBoxTex); // Left
            GUI.DrawTexture(new Rect(rect.xMax - borderThickness, rect.yMin, borderThickness, rect.height), selectionBoxTex); // Right
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMax - borderThickness, rect.width, borderThickness), selectionBoxTex); // Bottom
            GUI.color = Color.white;
        }
    }
}