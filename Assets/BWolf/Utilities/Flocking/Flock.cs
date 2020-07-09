using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    public class Flock : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField]
        private int unitCount = 5;

        [SerializeField]
        private float spawnPadding = 5f;

        [Header("Flock Settings")]
        [SerializeField]
        private float driveFactor = 10f;

        [SerializeField]
        private float maxSpeed = 5f;

        [SerializeField]
        private float neighborRadius = 1.5f;

        [SerializeField]
        private float avoidanceRadiusMultiplier = 0.5f;

        [Header("References")]
        [SerializeField]
        private GameObject prefabFlockUnit = null;

        [SerializeField]
        private FlockBehaviour behaviour = null;

        [SerializeField]
        private Collider terrainBound = null;

        public float SqrAvoidanceRadius { get; private set; }

        private List<FlockUnit> flockUnits = new List<FlockUnit>();

        private float sqrMaxSpeed;
        private float sqrNeighborRadius;

        private void Awake()
        {
            sqrMaxSpeed = maxSpeed * maxSpeed;
            sqrNeighborRadius = neighborRadius * neighborRadius;
            SqrAvoidanceRadius = sqrNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        }

        private void Start()
        {
            Bounds bounds = terrainBound.bounds;
            Vector3 position = terrainBound.transform.position - (bounds.size * 0.5f);
            for (int count = 0; count < unitCount; count++)
            {
                float x = Random.Range(position.x + spawnPadding, position.x + bounds.size.x - spawnPadding);
                float z = Random.Range(position.z + spawnPadding, position.z + bounds.size.z - spawnPadding);
                Vector3 p = new Vector3(x, 0, z);

                float y = Random.Range(0, 360f);
                Vector3 r = new Vector3(0, y, 0);
                flockUnits.Add(Instantiate(prefabFlockUnit, p, Quaternion.Euler(r)).GetComponent<FlockUnit>());
            }
        }

        private void Update()
        {
            Bounds bounds = terrainBound.bounds;
            foreach (FlockUnit unit in flockUnits)
            {
                MoveUnitBasedOnContext(unit);
            }
        }

        private void MoveUnitBasedOnContext(FlockUnit unit)
        {
            List<Transform> context = unit.GetContext(neighborRadius);
            Vector3 move = behaviour.CalculateMove(unit, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > sqrMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            move.y = 0;
            unit.Move(move);
        }
    }
}