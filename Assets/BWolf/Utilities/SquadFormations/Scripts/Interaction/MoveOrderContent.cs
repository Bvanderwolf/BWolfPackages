// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using BWolf.Utilities.SquadFormations.Units;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Interactions
{
    /// <summary>Content describing a move order given by the user</summary>
    public readonly struct MoveOrderContent
    {
        public readonly Vector3 WayPoint;
        public readonly List<Unit> OrderedUnits;

        public MoveOrderContent(Vector3 wayPoint, List<Unit> units)
        {
            WayPoint = wayPoint;
            OrderedUnits = units;
        }
    }
}