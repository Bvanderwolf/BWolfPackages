using BWolf.Examples.SquadFormations.Selection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class UnitGroupManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabFormation = null;

        private Dictionary<int, UnitGroup> groups = new Dictionary<int, UnitGroup>();

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
            if (units.Count > 1)
            {
                foreach (Unit unit in units)
                {
                    if (groups.ContainsKey(unit.AssignedGroupId))
                    {
                        groups[unit.AssignedGroupId].RemoveUnit(unit);
                    }

                    unit.ResetValues();
                }

                foreach (UnitGroup g in groups.Values)
                {
                    if (g.EnlistedUnits.Count == 1)
                    {
                        g.EnlistedUnits.RemoveAt(0);
                    }
                }

                UnitGroup group;
                if (!TryGetGroup(out group))
                {
                    int id = groups.Count;
                    group = new UnitGroup(id, units, Instantiate(prefabFormation).GetComponent<UnitFormation>());
                    groups.Add(id, group);
                }
            }
        }

        private bool TryGetGroup(out UnitGroup group)
        {
            if (groups.Count == 0)
            {
                group = null;
                return false;
            }

            group = groups.Values.FirstOrDefault(g => g.EnlistedUnits.Count == 0);

            return group != null;
        }
    }
}