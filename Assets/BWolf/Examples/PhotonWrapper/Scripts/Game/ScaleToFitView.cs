using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class ScaleToFitView : MonoBehaviour
    {
        [SerializeField]
        private Camera cam = null;

        /// <summary>Clamps this scale</summary>
        public void ClampToView()
        {
            float distance = (transform.position - cam.transform.position).magnitude;
            float height = 2.0f * Mathf.Tan(0.5f * cam.fieldOfView * Mathf.Deg2Rad) * distance;
            float width = height * cam.aspect;
            transform.localScale = new Vector3(width * 0.1f, 1, height * 0.1f);
        }
    }
}