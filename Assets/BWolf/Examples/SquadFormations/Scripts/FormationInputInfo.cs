using BWolf.Utilities.SquadFormations.Units;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BWolf.Examples.SquadFormations
{
    public class FormationInputInfo : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text txtName = null;

        [SerializeField]
        private TMP_Text txtKey = null;

        private KeyCode input;
        private string nameOfSetting;

        private bool setup = false;

        /// <summary>Setups up formation input info with name of setting and key to activate it</summary>
        public void Setup(string name, KeyCode key)
        {
            nameOfSetting = name;
            input = key;

            txtName.text = name;
            txtKey.text = key.ToString();

            setup = true;
        }

        private void Update()
        {
            if (setup && Input.GetKeyDown(input))
            {
                List<UnitGroup> selectedGroups = UnitGroupHandler.GetSelectedGroups();
                foreach (UnitGroup group in selectedGroups)
                {
                    group.Formation.UpdateSetting(nameOfSetting);
                }
            }
        }
    }
}