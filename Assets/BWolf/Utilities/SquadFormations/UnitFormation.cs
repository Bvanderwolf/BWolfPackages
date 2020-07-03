using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class UnitFormation
    {
        private Vector3 orientation;
        private State state;
        private int unitCount;

        private List<Vector3> positions;
        private List<Vector3> orientations;

        private enum State
        {
            Broken,
            Forming,
            Formed
        }
    }
}