using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class DragHandler : MonoBehaviour
    {
        [SerializeField]
        private Camera cam = null;

        private IDraggable currentDraggable;

        private GameStateManager gameStateManager;

        private void Awake()
        {
            gameStateManager = GetComponent<GameStateManager>();
        }

        private void Update()
        {
            if (gameStateManager.State != GameState.Playing || !TurnManager.HasTurn) { return; }

            if (Input.GetMouseButtonDown(0))
            {
                //set draggable if player clicked on it and is it draggable
                if (currentDraggable == null)
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Pawn")))
                    {
                        IDraggable draggableObject = hit.collider.GetComponentInParent<IDraggable>();
                        if (draggableObject.IsDraggable)
                        {
                            currentDraggable = draggableObject;
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0) && currentDraggable != null)
            {
                //drag draggable if the player is holding the left mouse button down and has a draggable
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("GameBoard")))
                {
                    currentDraggable.Drag(hit.point);
                }
            }

            if (Input.GetMouseButtonUp(0) && currentDraggable != null)
            {
                //release and reset the draggable member if the mouse button is up and there was a draggable stored
                currentDraggable.Release();
                currentDraggable = null;
            }
        }
    }
}