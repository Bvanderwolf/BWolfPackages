using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class UnitGroup
    {
        private int groupId;
        private int formationId;
        private int unitCount;
        private float maxSpeed;

        private List<GameObject> units;
        private List<Vector3> desiredPositions;
        private GameObject centroid;
        private GameObject commander;

        public void AddUnit(GameObject unit)
        {
        }

        public void RemoveUnit(GameObject unit)
        {
        }
    }
}