// Created By: Ties van Kipshagen @ https://www.tiesvankipshagen.com/
//----------------------------------

using UnityEngine;

namespace BWolf.Examples.SquadFormations
{
    public class CameraController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float panSpeed = 0.3f;

        [SerializeField]
        private float panSpeedMultiplier = 5;

        [SerializeField]
        private float camDistanceMin = 10f;

        [SerializeField]
        private float camDistanceMax = 50f;

        [SerializeField]
        private float camDistanceDefault = 30f;

        [SerializeField]
        private float camDistanceDelta = 3f;

        [SerializeField]
        private float camTiltMin = 30f;

        [SerializeField]
        private float camTiltMax = 60f;

        [SerializeField]
        private float camTiltDefault = 50f;

        [SerializeField]
        private float camMouseTiltSensitivity = 2f;

        [SerializeField]
        private float camRotationDelta = 2f;

        [SerializeField]
        private float camMouseRotationSensitivity = 2f;

        private float camAngle = 0f;
        private float camRotation = 0f;
        private CameraInput input;

        public float CamDistanceMin
        {
            get { return camDistanceMin; }
        }

        public float CamDistanceMax
        {
            get { return camDistanceMax; }
        }

        public float CamDistance { get; private set; } = 0f;

        private void Start()
        {
            CamDistance = camDistanceDefault;
            camAngle = camTiltDefault;
            input = GetComponent<CameraInput>();
        }

        private void FixedUpdate()
        {
            Rotate(input.Rotation * camRotationDelta);

            Pan(input.PanDirection);
        }

        private void Update()
        {
            Scroll(input.ScrollDelta * camDistanceDelta);

            Rotate(input.MouseRotation * camMouseRotationSensitivity);

            Tilt(input.MouseTilt * camMouseTiltSensitivity);
        }

        /// <summary>Change camera distance from centerpoint with given delta</summary>
        private void Scroll(float distanceDelta)
        {
            CamDistance += distanceDelta;
            CamDistance = Mathf.Clamp(CamDistance, camDistanceMin, camDistanceMax);
        }

        /// <summary>Change camera rotation around centerpoint with given delta</summary>
        private void Rotate(float rotationDelta)
        {
            camRotation += rotationDelta;
            transform.rotation = Quaternion.Euler(0f, camRotation, 0f);
        }

        /// <summary>Change camera tilt (= angle between centerpoint and camera) with given delta </summary>
        private void Tilt(float angleDelta)
        {
            camAngle += angleDelta;
            camAngle = Mathf.Clamp(camAngle, camTiltMin, camTiltMax);
        }

        /// <summary>Move camera centerpoint position in given direction</summary>
        private void Pan(Vector3 direction)
        {
            transform.Translate(direction * panSpeed * (input.PanBoost ? panSpeedMultiplier : 1), Space.Self);
        }
    }
}