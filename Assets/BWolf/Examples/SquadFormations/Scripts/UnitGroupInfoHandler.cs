using BWolf.Utilities.SquadFormations.Units;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Examples.SquadFormations
{
    public class UnitGroupInfoHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabUnitGroupInfo = null;

        private List<UnitGroupInfo> infoComponents = new List<UnitGroupInfo>();
        private List<UnitGroup> groupsToDisplay = new List<UnitGroup>();

        private int groupCount = 0;

        private void Update()
        {
            if (UnitGroupHandler.GetGroup(groupCount) != null)
            {
                CreateGroupDisplay();
                groupCount++;
            }
        }

        /// <summary>Creates display for found group</summary>
        private void CreateGroupDisplay()
        {
            UnitGroup group = UnitGroupHandler.GetGroup(groupCount);
            groupsToDisplay.Add(group);

            UnitGroupInfo info = Instantiate(prefabUnitGroupInfo, transform).GetComponent<UnitGroupInfo>();
            infoComponents.Add(info);
            info.AssignGroupToDisplay(group);
        }
    }
}