using BWolf.Utilities.SquadFormations.Units;
using UnityEngine;

namespace BWolf.Examples.SquadFormations
{
    public class UnitFormationInfo : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabFormationInput = null;

        [SerializeField]
        private Transform layoutTransform = null;

        private int groupGrab;

        private void Update()
        {
            if (groupGrab == -1) { return; }

            if (UnitGroupHandler.GetGroup(groupGrab) != null)
            {
                CreateFormationInput();
                groupGrab = -1;
            }
        }

        /// <summary>creates formation input info for each stored formation setting</summary>
        private void CreateFormationInput()
        {
            int startingKey = (int)KeyCode.Alpha1;
            UnitGroup group = UnitGroupHandler.GetGroup(groupGrab);
            foreach (FormationSetting setting in group.Formation.StoredSettings)
            {
                KeyCode key = (KeyCode)startingKey;
                Instantiate(prefabFormationInput, layoutTransform).GetComponent<FormationInputInfo>().Setup(setting.Name, key);
                startingKey++;
            }
        }
    }
}