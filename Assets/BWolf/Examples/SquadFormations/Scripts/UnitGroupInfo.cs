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

        private void Update()
        {
            txtId.text = string.Format("id: {0}", groupToDisplay.GroupId);
            txtUnitCount.text = string.Format("Units: {0}", groupToDisplay.EnlistedUnits.Count);
            txtFormation.text = string.Format("Formatin: {0}", groupToDisplay.Formation.CurrentSetting.Name);
            txtCommander.text = string.Format("Commander: {0}", groupToDisplay.Commander.name);
        }
    }
}