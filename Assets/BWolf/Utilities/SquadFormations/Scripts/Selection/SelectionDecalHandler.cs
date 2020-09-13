// Created By: Ties van Kipshagen @ https://www.tiesvankipshagen.com/
//----------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Selection
{
    /// <summary>Component class managing selectin decals to be used on selected/hovered units</summary>
    public class SelectionDecalHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabSelectionDecal = null;

        private List<SelectionDecal> selectionDecals = new List<SelectionDecal>();

        /// <summary>Assigns a decal to given selectable object</summary>
        public void AssignSelectionDecal(SelectableObject selectableObject)
        {
            GetDecal().Setup(selectableObject);
        }

        /// <summary>Retracts the decal managing the given selectable object</summary>
        public void RetractSelectionDecal(SelectableObject selectable)
        {
            foreach (SelectionDecal decal in selectionDecals)
            {
                if (decal.AssignedSelectable == selectable)
                {
                    decal.Retract();
                    break;
                }
            }
        }

        /// <summary>Returns a decal, either stored on instantiated</summary>
        private SelectionDecal GetDecal()
        {
            SelectionDecal decal = selectionDecals.FirstOrDefault(s => !s.gameObject.activeInHierarchy);
            if (decal == null)
            {
                decal = GameObject.Instantiate(prefabSelectionDecal, transform).GetComponent<SelectionDecal>();
                selectionDecals.Add(decal);
            }
            return decal;
        }
    }
}