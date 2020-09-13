// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

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
                    //convert selected objects to units to try and give orders
                    List<Unit> units = selectedObjects.ToUnits();
                    if (units.Count == 0)
                    {
                        return;
                    }

                    if (units.Count > 1 && !units.FormAGroup())
                    {
                        //if unit count is greater than 1 and the units not already form a group, start a new group
                        units = unitGroupHandler.StartGroup(units, terrainPosition);
                    }

                    //create the content for the moveorder to give to each unit
                    MoveOrderContent content = new MoveOrderContent(terrainPosition, units);
                    foreach (Unit unit in units)
                    {
                        unit.OnMoveOrdered(content);
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