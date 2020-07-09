using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    public class Flock : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private int unitCount = 5;

        [SerializeField]
        private float spawnPadding = 5f;

        [Header("References")]
        [SerializeField]
        private GameObject prefabFlockUnit = null;

        [SerializeField]
        private FlockBehaviour behaviour = null;

        [SerializeField]
        private MeshRenderer terrain = null;

        private List<FlockUnit> agents = new List<FlockUnit>();

        private void Start()
        {
            Bounds bounds = terrain.bounds;
            Vector3 position = terrain.transform.position - (bounds.size * 0.5f);
            for (int count = 0; count < unitCount; count++)
            {
                float x = Random.Range(position.x + spawnPadding, position.x + bounds.size.x - spawnPadding);
                float z = Random.Range(position.z + spawnPadding, position.z + bounds.size.z - spawnPadding);
                Vector3 p = new Vector3(x, 0, z);

                float y = Random.Range(0, 360f);
                Vector3 r = new Vector3(0, y, 0);
                agents.Add(Instantiate(prefabFlockUnit, p, Quaternion.Euler(r)).GetComponent<FlockUnit>());
            }
        }
    }
}