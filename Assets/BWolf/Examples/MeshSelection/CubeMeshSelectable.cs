using UnityEngine;

namespace BWolf.MeshSelecting
{
    [RequireComponent(typeof(BoxCollider))]
    public class CubeMeshSelectable : MonoBehaviour, ISelectableMesh
    {
        private MeshRenderer _renderer;

        private Color _color;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _color = _renderer.material.color;
        }

        public void OnClick()
        {
            Debug.Log($"{name} is clicked");
        }

        public void OnSelect()
        {
            _renderer.material.color = Color.blue;
        }

        public void OnDeselect()
        {
            _renderer.material.color = _color;
        }
    }
}
