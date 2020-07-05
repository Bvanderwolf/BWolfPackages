using BWolf.Utilities.SquadFormations.Selection;
using BWolf.Utilities.SquadFormations.Units;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Interactions
{
    /// <summary>Class for handling the interactions done by the user on selected objects</summary>
    public class InteractionHandler : MonoBehaviour
    {
        private UnitGroupHandler unitGroupHandler = null;

        private List<SelectableObject> selectedObjects;

        private void Awake()
        {
            unitGroupHandler = GetComponent<UnitGroupHandler>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && selectedObjects != null)
            {
                Vector3 terrainPosition;
                if (RaycastTerrain(out terrainPosition))
                {
                    //check selected units to see whether to give a group order or not
                    List<Unit> units = selectedObjects.ToUnits();
                    bool isGroupMove = units.Count > 1;
                    if (isGroupMove && !units.IsAGroup())
                    {
                        //if a group order is made and the units are not already a group, start a new group
                        unitGroupHandler.StartGroup(units, terrainPosition);
                    }

                    //create the content for the moveorder to call the interact method on each selected object
                    MoveOrderContent content = new MoveOrderContent(terrainPosition, isGroupMove);
                    Interaction moveOrder = new Interaction(InteractionType.MoveOrder, content);
                    foreach (SelectableObject selectable in selectedObjects)
                    {
                        selectable.Interact(moveOrder);
                    }
                }
            }
        }

        /// <summary>Stores reference to selected objects if its not empty</summary>
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