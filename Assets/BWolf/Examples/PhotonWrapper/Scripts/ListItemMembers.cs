using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Examples.PhotonWrapper
{
    public class ListItemMembers : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] members = null;

        public readonly Dictionary<string, GameObject> MemberDictionary = new Dictionary<string, GameObject>();

        private void Awake()
        {
            foreach (GameObject member in members)
            {
                MemberDictionary.Add(member.name, member);
            }
        }
    }
}