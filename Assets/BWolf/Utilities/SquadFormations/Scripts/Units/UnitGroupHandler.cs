using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    public class UnitGroupHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabFormation = null;

        private Dictionary<int, UnitGroup> groups = new Dictionary<int, UnitGroup>();

        public void StartGroup(List<Unit> units)
        {
            CleanActiveGroups(units);

            UnitGroup group;
            if (!TryGetGroup(out group))
            {
                int id = groups.Count;
                group = new UnitGroup(id, units, Instantiate(prefabFormation).GetComponent<UnitFormation>());
                groups.Add(id, group);
            }
        }

        public void StartGroup(List<Unit> units, Vector3 position)
        {
            CleanActiveGroups(units);

            UnitGroup group;
            if (!TryGetGroup(out group))
            {
                int id = groups.Count;
                group = new UnitGroup(id, position, units, Instantiate(prefabFormation).GetComponent<UnitFormation>());
                groups.Add(id, group);
            }
        }

        private void CleanActiveGroups(List<Unit> units)
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