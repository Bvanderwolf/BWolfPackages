using System.Collections.Generic;
using UnityEngine;

namespace BWolf.CameraControl
{
    public class CameraPanning : ICameraTranslation
    {
        [SerializeField]
        private bool _useKeyboardInput = true;

        [SerializeField]
        private float _movementSpeed = 15f;
        
        [SerializeField]
        private bool _useScreenEdgeInput = true;

        [SerializeField]
        private float _screenEdgeSpeed = 15f;

        [SerializeField]
        private float _screenEdgeThickness = 75f;
        
        private Rect _NonPanningRect => Rect.MinMaxRect
        (
            _screenEdgeThickness,
            _screenEdgeThickness,
            Screen.width - _screenEdgeThickness,
            Screen.height - _screenEdgeThickness
        );

        private Vector3 _ScreenCenter => new Vector3
        (
            Screen.width * 0.5f,
            Screen.height * 0.5f
        );

        private readonly Dictionary<KeyCode, Vector3> _panningKeyVectorMap = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.W, Vector3.forward },
            { KeyCode.A, Vector3.left },
            { KeyCode.S, Vector3.back },
            { KeyCode.D, Vector3.right },
        };

        public void Translate(Camera camera)
        {
            Transform transform = camera.transform;
            Vector3 vector = Vector3.zero;

            // Add key input to vector.
            foreach (KeyValuePair<KeyCode, Vector3> item in _panningKeyVectorMap)
                if (Input.GetKey(item.Key))
                    vector += (item.Value * (_movementSpeed * Time.deltaTime));

            // We don't want key panning and mouse panning to conflict.
            if (vector != Vector3.zero)
            {
                transform.position += vector;
                return ;
            }
            
            // Add mouse position to vector.
            Vector3 mousePosition = Input.mousePosition;
            if (!_NonPanningRect.Contains(mousePosition))
            {
                // Get the screen direction and convert it to lateral direction.
                Vector3 screenDirection = (mousePosition - _ScreenCenter).normalized;
                screenDirection.z = screenDirection.y;
                screenDirection.y = 0f;
                vector += screenDirection * (_screenEdgeSpeed * Time.deltaTime);
            }

            transform.position += vector;
        }
    }
}
