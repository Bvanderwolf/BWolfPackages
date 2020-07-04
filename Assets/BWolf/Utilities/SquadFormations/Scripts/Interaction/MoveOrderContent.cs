using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Interactions
{
    public struct MoveOrderContent
    {
        public readonly Vector3 WayPoint;
        public readonly bool GroupMove;

        public MoveOrderContent(Vector3 wayPoint, bool groupMove)
        {
            WayPoint = wayPoint;
            GroupMove = groupMove;
        }
    }
}