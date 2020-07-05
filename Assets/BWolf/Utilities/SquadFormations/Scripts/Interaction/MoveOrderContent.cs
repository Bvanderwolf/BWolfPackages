using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Interactions
{
    /// <summary>Content describing a move order given by the user</summary>
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