using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Selection
{
    public class SelectionDecalHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabSelectionDecal = null;

        private List<SelectionDecal> selectionDecals = new List<SelectionDecal>();

        public void AssignSelectionDecal(SelectableObject selectableObject)
        {
            SelectionDecal decal = GetDecal();
            decal.Setup(selectableObject);
        }

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