using BWolf.Utilities.Flocking.Behaviours;
using BWolf.Utilities.Flocking.Context;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    /// <summary>class for spawning and updating flock units</summary>
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
        private float neighbourRadius = 3f;

        [SerializeField]
        private float avoidanceRadiusMultiplier = 0.75f;

        [Header("References")]
        [SerializeField]
        private GameObject prefabFlockUnit = null;

        [SerializeField]
        private FlockBehaviour behaviour = null;

        [SerializeField]
        private BoxCollider terrainBound = null;

        [HideInInspector]
        public Vector3 Velocity;

        public float SqrAvoidanceRadius { get; private set; }
        public Bounds FlockBounds { get; private set; }

        private List<FlockUnit> flockUnits = new List<FlockUnit>();
        private float sqrMaxSpeed;

        private void Awake()
        {
            sqrMaxSpeed = maxSpeed * maxSpeed;
            SqrAvoidanceRadius = (neighbourRadius * neighbourRadius) * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

            FlockBounds = terrainBound.bounds;
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
            foreach (FlockUnit unit in flockUnits)
            {
                List<ContextItem> context = unit.GetContext(neighbourRadius);
                Vector3 step = behaviour.CalculateStep(unit, context, this);
                step *= driveFactor;
                if (step.sqrMagnitude > sqrMaxSpeed)
                {
                    step = step.normalized * maxSpeed;
                }

                step.y = 0;

                if (step != Vector3.zero)
                {
                    unit.Flock(step);
                }
            }
        }
    }
}