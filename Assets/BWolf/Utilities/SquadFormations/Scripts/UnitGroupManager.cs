using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class UnitGroupManager : MonoBehaviour
    {
        public static UnitGroupManager Instance { get; private set; }

        [SerializeField]
        private GameObject prefabFormation = null;

        private List<UnitGroup> groups = new List<UnitGroup>();
        private List<Unit> unitsStartingGroup = new List<Unit>();

        private void Awake()
        {
            Instance = this;
        }

        public void StartGroup(Unit unit)
        {
            unitsStartingGroup.Add(unit);
        }

        private void Update()
        {
            if (unitsStartingGroup.Count != 0)
            {
                groups.Add(new UnitGroup(unitsStartingGroup, Instantiate(prefabFormation).GetComponent<UnitFormation>()));
                unitsStartingGroup.Clear();
            }
        }
    }
}