using UnityEngine;

namespace BWolf.Examples.SquadFormations
{
    public class CameraInput : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private bool enableArrowsPanning = true;

        [SerializeField]
        private bool enableWASDPanning = true;

        [SerializeField]
        private bool enableEdgePanning = false;

        [SerializeField]
        private bool enableEdgePanningInEditor = false;

        [SerializeField]
        private bool enableQERotation = true;

        [SerializeField]
        private bool enableMouse3Rotation = true;

        [SerializeField]
        private bool enableMouse3Tilt = true;

        [SerializeField]
        private bool invertQERotation = false;

        [SerializeField]
        private bool invertMouse3Rotation = false;

        [SerializeField]
        private bool invertMouse3Tilt = false;

        [Header("References")]
        [SerializeField]
        private Camera cam = null;

        public Vector3 PanDirection { get; private set; }
        public bool PanBoost { get; private set; }
        public int ScrollDelta { get; private set; }
        public float Rotation { get; private set; }
        public float MouseRotation { get; private set; }
        public float MouseTilt { get; private set; }
        public bool GoToNearestUnit { get; private set; }

        private void Update()
        {
            PanDirection = GetPanDirection();

            PanBoost = Input.GetKey(KeyCode.LeftShift);

            ScrollDelta = GetScrollDelta();

            Rotation = GetRotation();

            MouseRotation = GetMouseRotation();

            MouseTilt = GetMouseTilt();

            GoToNearestUnit = Input.GetKey(KeyCode.Space);
        }

        /// <summary>Return the panning direction based on enabled input</summary>
        private Vector3 GetPanDirection()
        {
            Vector3 panDirection = Vector3.zero;

            if (enableArrowsPanning)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    panDirection += Vector3.right;
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    panDirection += Vector3.left;
                }

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    panDirection += Vector3.forward;
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    panDirection += Vector3.back;
                }
            }

            if (enableWASDPanning)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    panDirection += Vector3.right;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    panDirection += Vector3.left;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    panDirection += Vector3.forward;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    panDirection += Vector3.back;
                }
            }

            if (Application.isEditor ? enableEdgePanningInEditor : enableEdgePanning)
            {
                if (cam.ScreenToViewportPoint(Input.mousePosition).x > 1)
                {
                    panDirection += Vector3.right;
                }

                if (cam.ScreenToViewportPoint(Input.mousePosition).x < 0)
                {
                    panDirection += Vector3.left;
                }

                if (cam.ScreenToViewportPoint(Input.mousePosition).y > 1)
                {
                    panDirection += Vector3.forward;
                }

                if (cam.ScreenToViewportPoint(Input.mousePosition).y < 0)
                {
                    panDirection += Vector3.back;
                }
            }

            return panDirection;
        }

        /// <summary>Return the scroll delta input</summary>
        private int GetScrollDelta()
        {
            int scrollDelta = 0;

            if (Input.mouseScrollDelta.y > 0)
            {
                scrollDelta = -1;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                scrollDelta = 1;
            }

            return scrollDelta;
        }

        /// <summary>Return the rotation input</summary>
        private float GetRotation()
        {
            float rotation = 0;

            if (enableQERotation)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    rotation += (invertQERotation ? -1 : 1);
                }

                if (Input.GetKey(KeyCode.E))
                {
                    rotation += (invertQERotation ? 1 : -1);
                }
            }

            return rotation;
        }

        /// <summary>Return the mouse rotation input</summary>
        private float GetMouseRotation()
        {
            float mouseRotation = 0;

            if (enableMouse3Rotation)
            {
                if (Input.GetMouseButton(2))
                {
                    mouseRotation += Input.GetAxis("Mouse X") * (invertMouse3Rotation ? -1 : 1);
                }
            }

            return mouseRotation;
        }

        /// <summary>Return the mouse tilt input</summary>
        private float GetMouseTilt()
        {
            float mouseTilt = 0;

            if (enableMouse3Tilt)
            {
                if (Input.GetMouseButton(2))
                {
                    mouseTilt += Input.GetAxis("Mouse Y") * (invertMouse3Tilt ? 1 : -1);
                }
            }

            return mouseTilt;
        }
    }
}