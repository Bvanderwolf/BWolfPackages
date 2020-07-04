using BWolf.Examples.SquadFormations.Selection;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class UnitGroupManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabFormation = null;

        private List<UnitGroup> groups = new List<UnitGroup>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                List<Unit> units = new List<Unit>();
                foreach (SelectableObject s in SelectionHandler.Instance.SelectedObjects)
                {
                    Unit unit = s.GetComponent<Unit>();
                    if (unit != null)
                    {
                        units.Add(unit);
                    }
                }
                StartGroup(units);
            }
        }

        private void StartGroup(List<Unit> units)
        {
            groups.Add(new UnitGroup(units, Instantiate(prefabFormation).GetComponent<UnitFormation>()));
        }
    }
}