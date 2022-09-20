using System.Collections.Generic;
using UnityEngine;

namespace BWolf.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float _maxScrollHeight;
        
        [SerializeField]
        private float _panSpeed;

        [SerializeField]
        private float _scrollSpeed;

        [SerializeField]
        private float _panBorderThickness;

        private float _startingHeight;

        private Rect _NonPanningRect => Rect.MinMaxRect
        (
            _panBorderThickness,
            _panBorderThickness,
            Screen.width - _panBorderThickness,
            Screen.height - _panBorderThickness
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

        private void Start()
        {
            _startingHeight = transform.position.y;
        }

        private void Update()
        {
            Vector3 panningVector = GetPanningVector();
            Vector3 scrollVector = GetScrollVector();
            Vector3 position = transform.position + (panningVector + scrollVector);

            position.y = Mathf.Clamp(position.y, _startingHeight, (_startingHeight + _maxScrollHeight));
            transform.position = position;
        }

        private Vector3 GetScrollVector() => Input.mouseScrollDelta * (_scrollSpeed * Time.deltaTime);

        private Vector3 GetPanningVector()
        {
            Vector3 vector = Vector3.zero;

            // Add key input to vector.
            foreach (KeyValuePair<KeyCode, Vector3> item in _panningKeyVectorMap)
                if (Input.GetKey(item.Key))
                    vector += (item.Value * (_panSpeed * Time.deltaTime));

            // We don't want key panning and mouse panning to conflict.
            if (vector != Vector3.zero)
                return vector;
            
            // Add mouse position to vector.
            Vector3 mousePosition = Input.mousePosition;
            if (!_NonPanningRect.Contains(mousePosition))
            {
                // Get the screen direction and convert it to lateral direction.
                Vector3 screenDirection = (mousePosition - _ScreenCenter).normalized;
                screenDirection.z = screenDirection.y;
                screenDirection.y = 0f;
                vector += screenDirection * (_panSpeed * Time.deltaTime);
            }

            return vector;
        }
    }
}
