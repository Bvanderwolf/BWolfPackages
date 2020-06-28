using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class DragHandler : MonoBehaviour
    {
        [SerializeField]
        private Camera cam = null;

        [SerializeField, Tooltip("Offset on y-axis for pawns when being dragged")]
        private float draggOffset = 1f;

        private IDraggable draggable;

        private void Update()
        {
            //if (GameStateManager.Instance.State != GameState.Playing) { return; }

            if (Input.GetMouseButtonDown(0))
            {
                //set draggable if player clicked on it
                if (draggable == null)
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Pawn")))
                    {
                        draggable = hit.collider.GetComponentInParent<IDraggable>();
                    }
                }
            }

            if (Input.GetMouseButton(0) && draggable != null)
            {
                //drag draggable if the player is holding the left mouse button down and has a draggable
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("GameBoard")))
                {
                    draggable.Drag(hit.point);
                }
            }

            if (Input.GetMouseButtonUp(0) && draggable != null)
            {
                //release and reset the draggable member if the mouse button is up and there was a draggable stored
                draggable.Release();
                draggable = null;
            }
        }
    }
}