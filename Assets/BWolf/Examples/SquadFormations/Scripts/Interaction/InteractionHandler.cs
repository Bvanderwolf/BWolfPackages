using BWolf.Examples.SquadFormations.Selection;
using BWolf.Utilities.SquadFormations;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Examples.SquadFormations.Interactions
{
    public class InteractionHandler : MonoBehaviour
    {
        private List<SelectableObject> selectedObjects;

        private void Update()
        {
            if (Input.GetMouseButton(1) && selectedObjects != null)
            {
                Vector3 terrainPosition;
                if (RaycastTerrain(out terrainPosition))
                {
                    Interaction moveOrder = new Interaction(InteractionType.MoveOrder, terrainPosition);
                    foreach (SelectableObject selectable in selectedObjects)
                    {
                        selectable.Interact(moveOrder);
                    }
                }
            }
        }

        public void StoreSelectedObjects(List<SelectableObject> selectedObjects)
        {
            this.selectedObjects = selectedObjects.Count != 0 ? selectedObjects : null;
        }

        /// <summary>Helper method for raycasting on terrain</summary>
        private bool RaycastTerrain(out Vector3 terrainhit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Terrain")))
            {
                terrainhit = hit.point;
                return true;
            }

            terrainhit = Vector3.zero;
            return false;
        }
    }
}