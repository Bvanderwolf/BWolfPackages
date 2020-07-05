using BWolf.Utilities.SquadFormations.Units;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.SquadFormations
{
    public class UnitGroupInfo : MonoBehaviour
    {
        [SerializeField]
        private Text txtId = null;

        [SerializeField]
        private Text txtUnitCount = null;

        [SerializeField]
        private Text txtFormation = null;

        [SerializeField]
        private Text txtCommander = null;

        private UnitGroup groupToDisplay;

        public void AssignGroupToDisplay(UnitGroup group)
        {
            groupToDisplay = group;
        }
    }
}