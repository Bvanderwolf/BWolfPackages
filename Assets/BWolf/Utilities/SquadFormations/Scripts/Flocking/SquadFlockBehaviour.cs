using BWolf.Utilities.Flocking.Context;
using BWolf.Utilities.SquadFormations.Units;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Flocking
{
    public abstract class SquadFlockBehaviour : ScriptableObject
    {
        public abstract Vector3 CalculateStep(Unit unit, List<ContextItem> context, UnitGroupHandler handler);
    }
}