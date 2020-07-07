using BWolf.Utilities.SquadFormations.Selection;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>static class for adding usability functions for managing lists of formation positions, units and selectable objects</summary>
    public static class SquadFormationExtensions
    {
        /// <summary>Returns a sub list of unassigned formation positions</summary>
        public static List<FormationPosition> UnAssigned(this List<FormationPosition> positions)
        {
            List<FormationPosition> unAssigned = new List<FormationPosition>();
            foreach (FormationPosition p in positions)
            {
                if (!p.Assigned) { unAssigned.Add(p); }
            }

            return unAssigned;
        }

        public static Vector3[] Points(this List<FormationPosition> positions, bool local)
        {
            Vector3[] points = new Vector3[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                points[i] = local ? positions[i].transform.localPosition : positions[i].transform.position;
            }
            return points;
        }

        public static Vector3 Center(this List<FormationPosition> positions)
        {
            Vector3 center = Vector3.zero;
            foreach (FormationPosition formationPosition in positions)
            {
                center += formationPosition.transform.position;
            }
            return center / positions.Count;
        }

        public static bool GetSettingWithName(this List<FormationSetting> settings, string name, out FormationSetting setting)
        {
            for (int i = 0; i < settings.Count; i++)
            {
                if (settings[i].Name == name)
                {
                    setting = settings[i];
                    return true;
                }
            }
            setting = default;

            return false;
        }

        public static bool HasSettingWithName(this List<FormationSetting> settings, string name)
        {
            for (int i = 0; i < settings.Count; i++)
            {
                if (settings[i].Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public static void RemoveSetttingWithName(this List<FormationSetting> settings, string name)
        {
            for (int i = settings.Count - 1; i >= 0; i--)
            {
                if (settings[i].Name == name)
                {
                    settings.RemoveAt(i);
                    break;
                }
            }
        }

        public static FormationSetting Largest(this List<FormationSetting> settings)
        {
            if (settings.Count == 0) { return default; }

            FormationSetting setting = settings[0];
            for (int i = 1; i < settings.Count; i++)
            {
                if (settings[i].Size > setting.Size)
                {
                    setting = settings[i];
                }
            }
            return setting;
        }

        /// <summary>Returns a sub list of unassigned units</summary>
        public static List<Unit> UnAssigned(this List<Unit> units)
        {
            List<Unit> unAssigned = new List<Unit>();
            foreach (Unit u in units)
            {
                if (!u.IsAssignedAPosition) { unAssigned.Add(u); }
            }

            return unAssigned;
        }

        /// <summary>Returns whether this list of units all form a group</summary>
        public static bool FormAGroup(this List<Unit> units)
        {
            if (units.Count <= 1)
            {
                //return false if unit count is smaller than necessary count for making a group
                return false;
            }

            UnitGroup group = UnitGroupHandler.GetGroup(units[0].AssignedGroupId);

            if (group == null || group.EnlistedUnits.Count != units.Count)
            {
                //return false if group doesn't exit or enlisted unit count is not the same as given unit count
                return false;
            }

            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].AssignedGroupId != group.GroupId)
                {
                    //if any units isn't it the group, return false
                    return false;
                }
            }

            return true;
        }

        /// <summary>Converts this list to a list of units by checking for unit components. will be empty when no unit has been found</summary>
        public static List<Unit> ToUnits(this List<SelectableObject> selectables)
        {
            List<Unit> units = new List<Unit>();
            foreach (SelectableObject selectable in selectables)
            {
                Unit u = selectable.GetComponent<Unit>();
                if (u != null)
                {
                    units.Add(u);
                }
            }

            return units;
        }
    }
}